using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // GetComponent<SpriteRenderer>().color = Color.red;

        for(int i = 0; i < 1; i++) {
            for(int j = 0; j<42; j++) {
                break;
            }

            Debug.Log("caca");
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
