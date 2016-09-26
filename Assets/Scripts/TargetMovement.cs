using UnityEngine;
using System.Collections;

public class TargetMovement : BaseMovement {

    [Range(0,360)]
    public float angleOffset;
    public bool updateDir;

    private Transform player;
    private Vector3 currentDir;
    private Vector3 intialDir;
    private float initalSpeed = 0;
	
    void OnEnable() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentDir = player.position - transform.position;
        currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;

        if(initalSpeed == 0) {
            initalSpeed = speed;
        }
        speed = initalSpeed;
    }

	void Update () {
        if(player == null) {
            return;
        }

        if (updateDir) {
            currentDir = player.position - _transform.position;
            currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
        }

        _transform.up = -(player.position - _transform.position); // "hack" to make it face the player
        _transform.position += currentDir.normalized * speed * Time.deltaTime;
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
