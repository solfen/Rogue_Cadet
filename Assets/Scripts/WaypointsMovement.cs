using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointsMovement : MonoBehaviour {

    public List<Vector3> wayPoints;
    public float speed;

    private int currentPoint = 0;
    private Vector3 direction;
    private Transform _transform;

    // Use this for initialization
    void Start () {
        _transform = GetComponent<Transform>();
        for(int i = 0; i < wayPoints.Count; i++) {
            wayPoints[i] += _transform.position;
        }
    }

    void Update() {
        if(Vector3.Distance(_transform.position, wayPoints[currentPoint]) < 0.1f) {
            currentPoint = (currentPoint + 1) % wayPoints.Count;
        }

        direction = wayPoints[currentPoint] - _transform.position;
        _transform.position += direction.normalized * speed * Time.deltaTime;
    }
}
