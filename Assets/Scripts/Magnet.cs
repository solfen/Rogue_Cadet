using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

    public float speed;

    private Transform _transform;
    private Transform playerTransform;
    private bool attracted = false;
    private Vector3 direction = new Vector3();

    void Start () {
        _transform = transform.parent.GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (attracted) {
            direction = playerTransform.position - _transform.position;
            _transform.position += direction.normalized * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        attracted = true;
    }

    void OnTriggerExit2D() {
        attracted = false;
    }
}
