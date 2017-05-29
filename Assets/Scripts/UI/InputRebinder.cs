using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIPosAnimator))]
public class InputRebinder : MonoBehaviour {

    private KeyCode[] sniffedAxis = new KeyCode[2];
    private KeyCode lastKeyPressed = KeyCode.None;
    private string lastAxisPressed = "";
    private Vector2 UIUpPos = new Vector2(0, 350);
    private Vector2 UIDownPos = new Vector2(0, -350);
    private Vector2 UINormalPos = new Vector2(0, 0);
    private GameObject lastUISelected;

    [SerializeField] private GameObject overlay;
    [SerializeField] private MainMenuPanes bindPane;
    [SerializeField] private UIPosAnimator inputUI;
    [SerializeField] private Text inputUIText;

    public void StartRebinding() {
        InputManager.isRebinding = true;
        overlay.SetActive(true);
        bindPane.Open();
        StartCoroutine(Rebinding());
    }

    IEnumerator Rebinding() {
        inputUIText.text = InputManager.useGamedad ? ((InputManager.GameButtonID)(0)).ToString() : LocalizationManager.GetLocalizedText("INPUT_PANE_UP");

        if (!InputManager.useGamedad) {
            yield return inputUI.StartCoroutine(inputUI.Animate("Normal", UINormalPos));
            yield return StartCoroutine(SniffAxesInput(InputManager.GameAxisID.MOVE_Y, 0));

            yield return StartCoroutine(WaitForAnimAndKeyUp());
            inputUIText.text = LocalizationManager.GetLocalizedText("INPUT_PANE_DOWN");
            yield return inputUI.StartCoroutine(inputUI.Animate("Normal", UINormalPos));
            yield return StartCoroutine(SniffAxesInput(InputManager.GameAxisID.MOVE_Y, 1));
            InputManager.BindAxisInput(InputManager.GameAxisID.MOVE_Y, sniffedAxis[0], sniffedAxis[1]);

            yield return StartCoroutine(WaitForAnimAndKeyUp());
            inputUIText.text = LocalizationManager.GetLocalizedText("INPUT_PANE_RIGHT");
            yield return inputUI.StartCoroutine(inputUI.Animate("Normal", UINormalPos));
            yield return StartCoroutine(SniffAxesInput(InputManager.GameAxisID.MOVE_X, 0));

            yield return StartCoroutine(WaitForAnimAndKeyUp());
            inputUIText.text = LocalizationManager.GetLocalizedText("INPUT_PANE_LEFT");
            yield return inputUI.StartCoroutine(inputUI.Animate("Normal", UINormalPos));
            yield return StartCoroutine(SniffAxesInput(InputManager.GameAxisID.MOVE_X, 1));

            InputManager.BindAxisInput(InputManager.GameAxisID.MOVE_X, sniffedAxis[0], sniffedAxis[1]);
            yield return StartCoroutine(WaitForAnimAndKeyUp());
        }

        foreach (InputManager.GameButtonID id in System.Enum.GetValues(typeof(InputManager.GameButtonID))) {
            inputUIText.text = LocalizationManager.GetLocalizedText("INPUT_PANE_" + id);
            yield return inputUI.StartCoroutine(inputUI.Animate("Normal", UINormalPos));
            yield return StartCoroutine(SniffInput(id));
            yield return StartCoroutine(WaitForAnimAndKeyUp());
        }

        StartCoroutine(Close());
    }

    IEnumerator Close() {
        bindPane.Close();
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
        yield return new WaitForSecondsRealtime(0.15f);
        InputManager.isRebinding = false;

        overlay.SetActive(false);
    }

    IEnumerator WaitForAnimAndKeyUp() {
        if (lastKeyPressed != KeyCode.None) {
            while (!Input.GetKeyUp(lastKeyPressed)) {
                yield return null;
            }
        }
        else {
            while (Input.GetAxisRaw(lastAxisPressed) >= 0.5) {
                yield return null;
            }
        }

        yield return inputUI.StartCoroutine(inputUI.Animate("Down", UIDownPos));
        yield return inputUI.StartCoroutine(inputUI.Animate("Up", UIUpPos));
    }

    IEnumerator SniffInput(InputManager.GameButtonID id) {
        while (!Input.anyKeyDown && Input.GetAxisRaw("RightTrigger") < 0.5 && Input.GetAxisRaw("LeftTrigger") < 0.5) {
            yield return null;
        }

        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")) {
            StopAllCoroutines();
            StartCoroutine(Close());
            yield break;
        }

        if (Input.GetButtonDown("Map")) {
            yield return null;
        }

        lastKeyPressed = KeyCode.None;
        lastAxisPressed = "";

        if (Input.GetAxisRaw("LeftTrigger") >= 0.5) {
            lastAxisPressed = "LeftTrigger";
            InputManager.BindButtonInput(id, lastAxisPressed);
        }
        else if (Input.GetAxisRaw("RightTrigger") >= 0.5) {
            lastAxisPressed = "RightTrigger";
            InputManager.BindButtonInput(id, lastAxisPressed);
        }
        else {
            lastKeyPressed = InputManager.SniffKeyPressed();
            InputManager.BindButtonInput(id, lastKeyPressed);
        }
    }

    IEnumerator SniffAxesInput(InputManager.GameAxisID id, int axisKeyIndex) {
        while (!Input.anyKeyDown) {
            yield return null;
        }

        if(Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause")) {
            Debug.Log("wut");
            StopAllCoroutines();
            StartCoroutine(Close());
            yield break;
        }

        if(!Input.GetButtonDown("Map")) {
            lastKeyPressed = InputManager.SniffKeyPressed();
            sniffedAxis[axisKeyIndex] = lastKeyPressed;
        }
    }
}
