using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMapUI : MonoBehaviour {
    public static InputMapUI instance;

    public Sprite gamepadSprite;
    public Sprite keyboardSprite;
    public Image inputImage;
    public Text text;

    public bool isGamepad;

    private Animator anim;
    private bool isLoaded = false;

    void Awake () {
        isGamepad = Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "";
        inputImage.sprite = isGamepad ? gamepadSprite : keyboardSprite;
        anim = GetComponent<Animator>();
        instance = this;
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

    public void OnLoaded() {
        text.text = "Press start or space";
        isLoaded = true;
    }

    public void OnOpen() {
        Application.LoadLevel(0);
    }
}
