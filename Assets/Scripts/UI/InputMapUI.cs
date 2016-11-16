using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMapUI : MonoBehaviour {
    public static InputMapUI instance;

    public GameObject gamepadControls;
    public GameObject keyboardControls;
    public Text text;

    public bool isGamepad;

    private Animator anim;
    private bool isLoaded = false;

    void Awake () {
        anim = GetComponent<Animator>();
        instance = this;
        isGamepad = Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "";

        gamepadControls.SetActive(isGamepad);
        keyboardControls.SetActive(!isGamepad);

        EventDispatcher.AddEventListener(Events.GAME_LOADED, OnLoaded);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.GAME_LOADED, OnLoaded);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetButtonDown("Start") && isLoaded) {
            anim.SetTrigger("Close");
            isLoaded = false;
            Time.timeScale = 1;
        }
	}

    public void Open() {
        anim.SetTrigger("Open");
        Time.timeScale = 0;
    }

    public void OnLoaded(object useless) {
        text.text = "Press start or space";
        isLoaded = true;
    }

    public void OnOpen() {
        Application.LoadLevel(0);
    }
}
