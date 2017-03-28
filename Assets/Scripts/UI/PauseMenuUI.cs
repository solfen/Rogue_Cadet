using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PauseMenuUI : MonoBehaviour {

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectable;
    [SerializeField] private MainMenuPanes associatedPane;
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
        
        if(Input.GetButtonDown("Pause")) {
            if(!associatedPane.isOpen && !isOpen && Time.timeScale != 0) {
                isOpen = true;
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                Input.ResetInputAxes();
                associatedPane.Open();
            }
        }

        if(!associatedPane.isOpen && isOpen) {
            Time.timeScale = previousTimeScale;
            isOpen = false;
        }
	}

    private void OnGameStarted(object useless) {
        StartCoroutine(EnableDelay());
    }

    IEnumerator EnableDelay() {
        yield return new WaitForSeconds(0.5f);
        isLoaded = true;
    }

    public void Quit() {
        Application.Quit();
    }
}
