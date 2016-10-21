using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointsMovement : BaseMovement {

    public List<Vector3> wayPoints;
    private int currentPoint = 0;
    private Vector3 direction;

    // Use this for initialization
    void Start () {
        for(int i = 0; i < wayPoints.Count; i++) {
            wayPoints[i] = _transform.position + _transform.rotation * (wayPoints[i]);
        }

        SelectNewPoint();
    }

    void FixedUpdate() {
        if(Vector3.Distance(_transform.position, wayPoints[currentPoint]) < 0.1f) {
            SelectNewPoint();
        }

        _rigidbody.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            SelectNewPoint();
        }
    }

    private void SelectNewPoint() {
        currentPoint = (currentPoint + 1) % wayPoints.Count;
        direction = wayPoints[currentPoint] - _transform.position;
        direction.Normalize();
    }
}
