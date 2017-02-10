using UnityEngine;
using System.Collections;

public class ToggleObjectBasedOnGamepad : MonoBehaviour {

    [SerializeField] private GameObject gamepadObject;
    [SerializeField] private GameObject keyboardObject;

    void Start () {
        OnInputDeviceChanged(null);
        EventDispatcher.AddEventListener(Events.INPUT_DEVICE_CHANGED, OnInputDeviceChanged);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.INPUT_DEVICE_CHANGED, OnInputDeviceChanged);
    }

    private void OnInputDeviceChanged(object useless) {
        gamepadObject.SetActive(InputManager.useGamedad);
        keyboardObject.SetActive(!InputManager.useGamedad);
    }
}
