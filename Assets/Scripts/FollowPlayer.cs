using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Transform _transform;
    private Transform playerTransform;
    private Vector3 newPos = new Vector3();

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        newPos.z = _transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        if(playerTransform != null) {
            newPos.Set(playerTransform.position.x, playerTransform.position.y, newPos.z);
            _transform.position = newPos;
        }
    }
}
