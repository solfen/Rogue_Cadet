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

    private int currentPoint = 0;
    private float timer = 0;
    private bool isTeleporting = false;
    public Rigidbody2D _rigidBody;

    void Start() {
        timer = teleportInterval;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(!isTeleporting) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                anim.SetTrigger("Teleport");
                timer = teleportInterval;
                _rigidBody.simulated = false;
                isTeleporting = true;
                if(weapon != null) {
                    ((ISwitchable)weapon).SwitchState(false);
                }
            }
        }
    }

    //Called from animation;
    public void ChangePos() {
        _rigidBody.simulated = true;

        if (isRandom) {
            _transform.position =  new Vector3(_transform.position.x + Random.Range(-teleportMaxDistance, teleportMaxDistance), _transform.position.y + Random.Range(-teleportMaxDistance, teleportMaxDistance), 0);
        }
        else {
            currentPoint = (currentPoint + 1) % points.Count;
            _transform.position = points[currentPoint];
        }
    }

    //Called from animation;
    public void TeleportAnimEnd() {
        isTeleporting = false;
        if (weapon != null)
            ((ISwitchable)weapon).SwitchState(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            ChangePos();
        }
    }
}
