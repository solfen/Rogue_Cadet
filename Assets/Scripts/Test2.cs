using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<SpriteRenderer>().color = Color.red;
        Debug.Log(Quaternion.Euler(0, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(GetComponentInChildren<Rigidbody2D>().IsSleeping());
        }
	}
}
