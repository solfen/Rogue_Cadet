using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetMovement : BaseMovement {

    [Range(0, 360)]
    [SerializeField]
    private float angleOffset;
    [Tooltip("Go toward a point at x distance before the player")]
    [SerializeField]
    private float distanceOffset;
    [SerializeField]
    private Vector3 posOffset;
    [Tooltip("distance of \"vision\"")]
    [SerializeField]
    private float detectionDistance = 7.5f;
    [Tooltip("Distance form the obstacels where the enemy move perpendicularly")]
    [SerializeField]
    private float minObstacleDist = 1f;
    [Tooltip("Collision detection is done with a box of the size of the collider multiplied by this")]
    [SerializeField]
    private float boxCastSizeMultiplier = 1.3f;

    private Transform playerTransform;
    private Player player;
    private Vector3 currentDir = new Vector3();
    private Quaternion offset;
    private int eyesMask = 1 << 13;
    private int playerMask = 1 << 10;
    private bool inWall = false;
    private Vector2 colliderSize;
    private Collider2D lastCollider;
    private float normalPerpendicularDir;

    protected override void Awake() {
        base.Awake();
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);

        playerMask = playerMask | eyesMask;
        offset = Quaternion.Euler(0, 0, angleOffset);
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        colliderSize = col != null ? col.size * boxCastSizeMultiplier * _transform.localScale.x : Vector2.zero;

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go == null) {
            EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
            enabled = false;
            canSwitch = false; //makes sure no external script can activate us untill initialized
            return;
        }

        Init(go.GetComponent<Player>());
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
    }

    private void Init(Player _player) {
        playerTransform = _player.GetComponent<Transform>();
        player = _player;
    }

    private void OnPlayerCreated(object playerObj) {
        Init((Player)playerObj);
        enabled = true;
        canSwitch = true;
    }

    public override void SwitchState(bool state) {
        if (!canSwitch) {
            return;
        }

        enabled = state;
        _rigidbody.velocity = Vector2.zero;
    }

    private void OnPlayerDeath(object useless) {
        SwitchState(false);
        canSwitch = false;
    }

    void FixedUpdate() {
        if (!player.isInvisible) {
            LaserEyes();
            _transform.up = _transform.position - playerTransform.position; // "hack" to make it face the player // actually it's bad, it sometimes makes them move in 3D...
            _rigidbody.velocity = currentDir * speed;
        }
        else {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void LaserEyes() {
        currentDir = playerTransform.position + posOffset - _transform.position;
        currentDir -= currentDir.normalized * distanceOffset;
        currentDir = offset * currentDir;

        //Debug.DrawRay(_transform.position, currentDir.normalized * detectionDistance, Color.red);

        RaycastHit2D playerHit = Physics2D.Raycast(_transform.position, currentDir, detectionDistance, playerMask);
        RaycastHit2D boxCastHit = Physics2D.BoxCast(_transform.position, colliderSize, transform.rotation.eulerAngles.z, currentDir, detectionDistance, eyesMask);
        float playerHitDist = (playerHit.collider == null || playerHit.collider.tag != "Player") ? Mathf.Infinity : playerHit.distance;

        if (boxCastHit.collider != null && boxCastHit.distance < playerHitDist) {
            if (inWall) { // emergency reverse
                currentDir = boxCastHit.normal;
                inWall = boxCastHit.distance <= 0;
            }
            else {
                if (boxCastHit.collider != lastCollider) { // select avoid direction for current collider
                    Vector3 p1 = new Vector3(-boxCastHit.normal.y, boxCastHit.normal.x, 0).normalized;
                    Vector3 p2 = new Vector3(boxCastHit.normal.y, -boxCastHit.normal.x, 0).normalized;

                    normalPerpendicularDir = (_transform.position + p1 - playerTransform.position).sqrMagnitude <= (_transform.position + p2 - playerTransform.position).sqrMagnitude ? -1 : 1;
                    lastCollider = boxCastHit.collider;
                }

                Vector3 perpendicular = new Vector3(boxCastHit.normal.y * normalPerpendicularDir, boxCastHit.normal.x * -normalPerpendicularDir, 0).normalized;
                currentDir = Vector3.Lerp(currentDir, perpendicular, 1 - ((boxCastHit.distance - minObstacleDist) / detectionDistance));
            }

            //Debug.DrawRay(boxCastHit.point, boxCastHit.normal * 100, Color.magenta);
        }
        else {
            lastCollider = null;
        }

        currentDir.Normalize();
        //Debug.DrawRay(_transform.position, currentDir * 1000, Color.blue);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            inWall = true;
        }
    }
}
