﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PauseMenuUI : MonoBehaviour {

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectable;
    [SerializeField] private bool waitForGameStart = true;

    private bool isLoaded = false;
    private bool isOpen = false;
    private Animator anim;
    private GameObject previousSelected;
    private float previousTimeScale = 1;

	void Start () {
        anim = GetComponent<Animator>();
        EventDispatcher.AddEventListener(Events.GAME_STARTED, OnGameStarted);
        isLoaded = !waitForGameStart;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.GAME_STARTED, OnGameStarted);
    }

    void Update () {
        if (!isLoaded)
            return;
        
        if(!InputManager.isRebinding && Input.GetButtonDown("Pause")) {
            if(!isOpen && Time.timeScale != 0) {
                Open();
            }
            else {
                Close();
            }
        }
	}

    private void OnGameStarted(object useless) {
        StartCoroutine(EnableDelay());
    }

    IEnumerator EnableDelay() {
        yield return new WaitForSeconds(0.5f);
        isLoaded = true;
    }

    private void Open() {
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        previousSelected = eventSystem.currentSelectedGameObject;
        previousTimeScale = Time.timeScale;

        anim.SetTrigger("Open");
        eventSystem.SetSelectedGameObject(firstSelectable);
        Time.timeScale = 0;
        isOpen = true;
    }

    public void Close() {
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
        anim.SetTrigger("Close");
        eventSystem.SetSelectedGameObject(previousSelected);
        Time.timeScale = previousTimeScale;
        isOpen = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
