using UnityEngine;
using System.Collections;

public class InitGame : MonoBehaviour {

    private bool isLoaded = false;

    // Use this for initialization
    void Start () {
        Time.timeScale = 0;
        EventDispatcher.AddEventListener(Events.GAME_LOADED, OnLoaded);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.GAME_LOADED, OnLoaded);
    }

    void Update() {
        if (Input.GetButtonDown("Start") && isLoaded) {
            Time.timeScale = 1;
            EventDispatcher.DispatchEvent(Events.GAME_STARTED, null);
            enabled = false;
        }
    }

    private void OnLoaded(object useless) {
        isLoaded = true;
    }
}
