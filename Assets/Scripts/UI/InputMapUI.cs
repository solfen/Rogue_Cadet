using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputMapUI : MonoBehaviour {
    [SerializeField] private GameObject gamepadControls;
    [SerializeField] private GameObject keyboardControls;
    [SerializeField] private Text text;

    private Animator anim;

    void Awake () {
        anim = GetComponent<Animator>();
        bool isGamepad = Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "";

        gamepadControls.SetActive(isGamepad);
        keyboardControls.SetActive(!isGamepad);

        EventDispatcher.AddEventListener(Events.GAME_LOADED, OnLoaded);
        EventDispatcher.AddEventListener(Events.GAME_STARTED, OnGameStart);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.GAME_LOADED, OnLoaded);
        EventDispatcher.RemoveEventListener(Events.GAME_STARTED, OnGameStart);
    }

    //called from UpgradeShop
    public void Open() {
        anim.SetTrigger("Open");
    }

    private void OnLoaded(object useless) {
        text.text = "Press start or space";
    }

    private void OnGameStart(object useless) {
        anim.SetTrigger("Close");
    }

    public void OnOpenAnimFinished() {
        SceneManager.LoadScene(0);
    }
}
