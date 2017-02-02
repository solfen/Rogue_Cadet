using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    public Text realText;

    void Update () {
        if (Input.GetButtonDown("Bomb")) {
            Application.LoadLevel(4);
        }
    }

}
