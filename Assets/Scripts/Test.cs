﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    public Text realText;

    void Update () {

        if (Input.GetButtonDown("Bomb")) {
            // Application.LoadLevel(4);
            GetComponent<InputRebinder>().StartRebinding();
        }

        if(InputManager.GetButtonDown(InputManager.GameButtonID.SHOOT)) {
            Debug.Log("caca");
        }

        //Debug.Log(InputManager.GetAxisRaw(InputManager.GameAxisID.MOVE_X));

        Debug.Log(InputManager.useGamedad);
    }

}
