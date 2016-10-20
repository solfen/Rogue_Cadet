using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportationMovement : BaseMovement {

    public bool isRandom;
    public float teleportMaxDistance;
    public List<Vector3> points;
    public float teleportInterval = 1;
    public Animator anim;

    private int currentPoint = 0;

    void Start() {
        StartCoroutine(TeleportLoop());
    }

    IEnumerator TeleportLoop() {
        while(true) {
            yield return new WaitForSeconds(teleportInterval);
            anim.SetTrigger("Teleport");
        }
    }

    //Called from animation;
    public void Teleport() {
        if(isRandom) {
            _transform.position =  new Vector3(_transform.position.x + Random.Range(-teleportMaxDistance, teleportMaxDistance), _transform.position.y + Random.Range(-teleportMaxDistance, teleportMaxDistance), 0);
        }
        else {
            currentPoint = (currentPoint + 1) % points.Count;
            _transform.position = points[currentPoint];
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            Teleport();
        }
    }
}
