﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

    public string test = "caca";
	// Use this for initialization

	void Start () {
        //GetComponent<SpriteRenderer>().color = Color.red;
        //EventDispatcher.AddEventListener(Events.TEST, OnTest);
        //EventDispatcher.DispatchEvent(Events.TEST, this);
    }

    private void OnTest(object emiter) {
        Debug.Log(((Test2)emiter).test);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(GetComponentInChildren<Rigidbody2D>().IsSleeping());
        }
	}
}