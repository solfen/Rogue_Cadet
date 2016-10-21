using UnityEngine;
using System.Collections;

public class TargetMovement : BaseMovement {

    [Range(0,360)]
    public float angleOffset;
    public Vector3 posOffset;
    public float minObstacleDist = 2f;
    public float emergencyDistance = 4;
    public Transform leftEye;
    public Transform rightEye;
    public float eyesLength = 10;
    public float eyeAngle = 20;
    public float laserInterval = 0.5f;

    private Transform player;
    private Rigidbody2D _rigidBody;
    private Vector3 currentDir;
    private Quaternion offset;
    private Quaternion leftEyeQuat;
    private Quaternion rightEyeQuat;
    private int eyesMask = 1 << 13;
    private int playerMask = 1 << 10;

    void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMask = playerMask | eyesMask;

        offset = Quaternion.Euler(0, 0, angleOffset);
        currentDir = player.position + posOffset - transform.position;
        currentDir = offset * currentDir;
        currentDir.Normalize();

        StartCoroutine(LaserEyes());
        

        leftEyeQuat = Quaternion.Euler(0, 0, eyeAngle);
        rightEyeQuat = Quaternion.Euler(0, 0, -eyeAngle);
    }

	void FixedUpdate () {
        if(player == null) {
            enabled = false;
            return;
        }

        _transform.up =  _transform.position - player.position; // "hack" to make it face the player
        _rigidBody.velocity = currentDir * speed;
    }

    IEnumerator LaserEyes() {
        while (true) {
            currentDir = player.position + posOffset - _transform.position;
            currentDir = offset * currentDir;

            //Debug.DrawRay(_transform.position, currentDir, Color.red);
            //Debug.DrawRay(leftEye.position, (leftEyeQuat * currentDir).normalized * eyesLength, Color.yellow, laserInterval);
            //Debug.DrawRay(rightEye.position, (rightEyeQuat * currentDir).normalized * eyesLength, Color.yellow, laserInterval);

            RaycastHit2D playerHit = Physics2D.Raycast(_transform.position, currentDir, eyesLength, playerMask);
            if(playerHit.collider == null || playerHit.collider.tag != "Player") {
                RaycastHit2D hit = Physics2D.Raycast(leftEye.position, leftEyeQuat * currentDir, eyesLength, eyesMask);
                RaycastHit2D hit2 = Physics2D.Raycast(rightEye.position, rightEyeQuat * currentDir, eyesLength, eyesMask);
                float minDist = hit.distance == 0 ? hit2.distance : hit2.distance == 0 ? hit.distance : Mathf.Min(hit.distance, hit2.distance);

                if(minDist > 0 && minDist < emergencyDistance) {
                    yield return StartCoroutine(EmergencyBreak());
                }
                else {
                    if (hit.collider != null) {
                        currentDir = Quaternion.Euler(0, 0, -Mathf.Lerp(10, 180, emergencyDistance / minDist)) * currentDir;
                    }
                    else if(hit2.collider != null) {
                        currentDir = Quaternion.Euler(0, 0, Mathf.Lerp(10, 180, emergencyDistance / minDist)) * currentDir;
                    }
                }

            }

            currentDir.Normalize();
            //Debug.DrawRay(_transform.position, currentDir * 1000, Color.blue);

            yield return new WaitForSeconds(laserInterval);
        }
    }

    IEnumerator EmergencyBreak() {
        currentDir = -currentDir;
        currentDir.Normalize();
        yield return new WaitForSeconds(0.5f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            _transform.position -= currentDir.normalized * speed * Time.deltaTime * 5;
        }
    }

}
