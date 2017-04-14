using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportationMovement : BaseMovement {

    public bool isRandom;
    public float teleportMaxDistance;
    public List<Vector3> points;
    public float teleportInterval = 1;
    public Animator anim;
    public WeaponEnemies weapon;
    public LayerMask teleportColissionCheck;
    public float collisionRadius;

    private int currentPoint = 0;
    private float timer = 0;
    private bool isTeleporting = false;
    private Collider2D[] cols = new Collider2D[1];

    void Start() {
        timer = teleportInterval;
    }

    void Update() {
        if(!isTeleporting) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                anim.SetTrigger("Teleport");
                timer = teleportInterval;
                _rigidbody.simulated = false;
                isTeleporting = true;
                if(weapon != null) {
                    ((ISwitchable)weapon).SwitchState(false);
                }
            }
        }
    }

    //Called from animation;
    public void ChangePos() {

        if (isRandom) {
            _transform.position =  new Vector3(_transform.position.x + Random.Range(-teleportMaxDistance, teleportMaxDistance), _transform.position.y + Random.Range(-teleportMaxDistance, teleportMaxDistance), 0);
            if(Physics2D.OverlapCircleNonAlloc(_transform.position, collisionRadius, cols, teleportColissionCheck.value) > 0) {
                ChangePos();
            }
        }
        else {
            currentPoint = (currentPoint + 1) % points.Count;
            _transform.position = points[currentPoint];
        }
    }

    //Called from animation;
    public void TeleportAnimEnd() {
        _rigidbody.simulated = true;

        isTeleporting = false;
        if (weapon != null)
            ((ISwitchable)weapon).SwitchState(true);
    }
}
