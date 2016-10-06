using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static bool useGamedad;

    // Use this for initialization
    void Awake () {
        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "") {
            useGamedad = true;
        }
    }
}
