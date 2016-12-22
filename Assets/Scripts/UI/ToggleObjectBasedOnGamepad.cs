using UnityEngine;
using System.Collections;

public class ToggleObjectBasedOnGamepad : MonoBehaviour {

    [SerializeField] private GameObject gamepadObject;
    [SerializeField] private GameObject keyboardObject;

    void Start () {
        gamepadObject.SetActive(InputManager.useGamedad);
        keyboardObject.SetActive(!InputManager.useGamedad);
    }

}
