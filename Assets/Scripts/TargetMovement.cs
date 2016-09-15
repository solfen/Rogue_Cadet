using UnityEngine;
using System.Collections;

public class TargetMovement : BaseMovement {

    [Range(0,360)]
    public float angleOffset;
    public bool updateDir;

    private Transform player;
    private Vector3 currentDir;
    private Vector3 intialDir;
	
    void OnEnable() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentDir = player.position - transform.position;
        currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
    }

	void Update () {
        if(player == null) {
            return;
        }

        if (updateDir) {
            currentDir = player.position - _transform.position;
            currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
        }

        _transform.position += currentDir.normalized * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            _transform.position -= currentDir.normalized * speed * Time.deltaTime * 10;
        }
    }

}
