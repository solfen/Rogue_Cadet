using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetMovement : BaseMovement {

    private enum AvoidanceDir {
        NONE,
        LEFT,
        RIGHT
    }

    [Range(0,360)]
    public float angleOffset;
    [Tooltip("Go toward a point at x distance before the player")]
    public float distanceOffset;
    public Vector3 posOffset;
    public Transform leftEye;
    public Transform rightEye;
    public float eyesLength = 7.5f;
    public float eyeAngle = 5;
    [Tooltip("The minimum time between a dir change. To avoid left/right change at each frame")]
    public float changeAvoidDirMinInterval = 0.3f;
    public float laserInterval = 0.5f;

    private Transform playerTransform;
    private Player player;
    private Vector3 currentDir = new Vector3();
    private Quaternion offset;
    private Quaternion leftEyeQuat;
    private Quaternion rightEyeQuat;
    private int eyesMask = 1 << 13;
    private int playerMask = 1 << 10;
    private Dungeon dungeon;
    private GraphRoom currentRoom;
    private KeyValuePair<AvoidanceDir, float> lastAvoidDirTime = new KeyValuePair<AvoidanceDir, float>();
    private AvoidanceDir dominantAvoidStrategy;

    protected override void Awake () {
        base.Awake();
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);

        playerMask = playerMask | eyesMask;
        leftEyeQuat = Quaternion.Euler(0, 0, eyeAngle);
        rightEyeQuat = Quaternion.Euler(0, 0, -eyeAngle);
        offset = Quaternion.Euler(0, 0, angleOffset);
        dominantAvoidStrategy = Random.value < 0.5f ? AvoidanceDir.RIGHT : AvoidanceDir.LEFT;

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go == null) {
            EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
            enabled = false;
            canSwitch = false;
            return;
        }

        GameObject dungeonGO = GameObject.FindGameObjectWithTag("Dungeon");
        if(dungeonGO != null) {
            dungeon = dungeonGO.GetComponent<Dungeon>();
        }

        Init(go.GetComponent<Player>());
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

        if (speed > 0) {
            if (state)
                StartCoroutine("LaserEyes");
            else
                StopCoroutine("LaserEyes");
        }

        enabled = state;
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
    }

    private void OnPlayerDeath(object useless) {
        SwitchState(false);
        canSwitch = false;
    }

	void FixedUpdate () {
        if (!player.isInvisible) {
            _transform.up = _transform.position - playerTransform.position; // "hack" to make it face the player // actually it's bad, it sometimes makes them move 3D...
            _rigidbody.velocity = currentDir * speed;

        }
        else {
            _rigidbody.velocity = Vector2.zero;
        }

        if(dungeon != null) {
            GraphRoom newRoom = dungeon.GetRoomFromPosition(_transform.position);
            if (newRoom != null && newRoom != currentRoom) {
                _transform.parent = newRoom.roomInstance.enemiesParent.GetChild(0);
                currentRoom = newRoom;
            }
        }
    }

    IEnumerator LaserEyes() {
        while (true) {
            currentDir = playerTransform.position + posOffset - _transform.position;
            currentDir -= currentDir.normalized * distanceOffset;
            currentDir = offset * currentDir;

            //Debug.DrawRay(_transform.position, currentDir, Color.red);
            //Debug.DrawRay(leftEye.position, (leftEyeQuat * currentDir).normalized * eyesLength, Color.yellow, laserInterval);
            //Debug.DrawRay(rightEye.position, (rightEyeQuat * currentDir).normalized * eyesLength, Color.yellow, laserInterval);

            RaycastHit2D playerHit = Physics2D.Raycast(_transform.position, currentDir, eyesLength, playerMask);
            if(playerHit.collider == null || playerHit.collider.tag != "Player") {
                RaycastHit2D hit = Physics2D.Raycast(leftEye.position, leftEyeQuat * currentDir, eyesLength, eyesMask);
                RaycastHit2D hit2 = Physics2D.Raycast(rightEye.position, rightEyeQuat * currentDir, eyesLength, eyesMask);
                float minDist = hit.distance == 0 ? hit2.distance : hit2.distance == 0 ? hit.distance : Mathf.Min(hit.distance, hit2.distance);

                if(dominantAvoidStrategy == AvoidanceDir.RIGHT)
                    CheckForObstacles(dominantAvoidStrategy, hit, hit2, minDist);
                else 
                    CheckForObstacles(dominantAvoidStrategy, hit2, hit, minDist);
            }
            else {
                ChangeAvoidDir(AvoidanceDir.NONE);
            }

            currentDir.Normalize();
            //Debug.DrawRay(_transform.position, currentDir * 1000, Color.blue);

            yield return new WaitForSeconds(laserInterval);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            currentDir = Quaternion.Euler(0, 0, 90) * currentDir;
        }
    }

    private void ChangeAvoidDir(AvoidanceDir newDir) {
        if(lastAvoidDirTime.Key != newDir) {
            lastAvoidDirTime = new KeyValuePair<AvoidanceDir, float>(newDir, Time.realtimeSinceStartup);
        }
    }

    private void CheckForObstacles(AvoidanceDir firstDir, RaycastHit2D hit1, RaycastHit2D hit2, float obstacleDist) {
        AvoidanceDir secondDir = firstDir == AvoidanceDir.RIGHT ? AvoidanceDir.LEFT : AvoidanceDir.RIGHT;

        if(hit1.collider != null && (lastAvoidDirTime.Key == firstDir || lastAvoidDirTime.Value + changeAvoidDirMinInterval < Time.realtimeSinceStartup)) {
            AvoidCollision(firstDir, obstacleDist);
        }
        else if (hit2.collider != null && (lastAvoidDirTime.Key == secondDir || lastAvoidDirTime.Value + changeAvoidDirMinInterval < Time.realtimeSinceStartup)) {
            AvoidCollision(secondDir, obstacleDist);
        }
        else {
            ChangeAvoidDir(AvoidanceDir.NONE);
        }
    }

    private void AvoidCollision(AvoidanceDir dir, float obstacleDist) {
        currentDir = Quaternion.Euler(0, 0, Mathf.Lerp(0, 180, 1 - (obstacleDist / eyeAngle)) * (dir == AvoidanceDir.RIGHT ? -1 : 1)) * currentDir;
        ChangeAvoidDir(dir);
    }
}
