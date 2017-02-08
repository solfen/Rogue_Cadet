using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRebinder : MonoBehaviour {

    public void StartRebinding() {
        StartCoroutine(Rebinding());
    }

    IEnumerator Rebinding() {
        yield return new WaitForSeconds(1); //tmp
        //yield for UI open animation time
        foreach (InputManager.GameButtonID id in System.Enum.GetValues(typeof(InputManager.GameButtonID))) {
            yield return StartCoroutine(SniffInput(id));
            //do UI anims
        }

        //close
    }

    IEnumerator SniffInput(InputManager.GameButtonID id) {
        while (!Input.anyKeyDown && Input.GetAxisRaw("RightTrigger") < 0.5 && Input.GetAxisRaw("LeftTrigger") < 0.5) {
            yield return null;
        }

        if (Input.GetAxisRaw("LeftTrigger") >= 0.5) {
            InputManager.BindButtonInput(id, "LeftTrigger");
        }
        else if (Input.GetAxisRaw("RightTrigger") >= 0.5) {
            InputManager.BindButtonInput(id, "RightTrigger");
        }
        else {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKey(vKey)) {
                    InputManager.BindButtonInput(id, vKey);
                }
            }
        }
    }
}
