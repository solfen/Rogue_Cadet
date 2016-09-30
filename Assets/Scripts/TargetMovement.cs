using UnityEngine;
using System.Collections;

public class TargetMovement : BaseMovement {

    [Range(0,360)]
    public float angleOffset;
    public bool updateDir;

    private Transform player;
    private Vector3 currentDir;
    private float initalSpeed = 0;
    private Quaternion offset;
	
    void OnEnable() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        offset = Quaternion.Euler(0, 0, angleOffset);
        currentDir = player.position - transform.position;
        currentDir = offset * currentDir;
        currentDir.Normalize();

        if (initalSpeed == 0) {
            initalSpeed = speed;
        }
        speed = initalSpeed;
    }

	void Update () {
        if(player == null) {
            enabled = false;
            return;
        }

        if (updateDir) {
            currentDir = player.position - _transform.position;
            currentDir = offset * currentDir;
            currentDir.Normalize();
        }

        _transform.up = -(player.position - _transform.position); // "hack" to make it face the player
        _transform.position += currentDir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            _transform.position -= currentDir.normalized * speed * Time.deltaTime * 5;
            if(!updateDir) {
                speed = 0;
            }
        }
    }

}
