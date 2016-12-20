using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PauseMenuUI : MonoBehaviour {

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelectable;

    private bool isLoaded = false;
    private bool isOpen = false;
    private Animator anim;
    private GameObject previousSelected;
    private float previousTimeScale = 1;

	void Start () {
        anim = GetComponent<Animator>();
        EventDispatcher.AddEventListener(Events.GAME_STARTED, OnGameStarted);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.GAME_STARTED, OnGameStarted);
    }

    void Update () {
        if (!isLoaded)
            return;
        
        if(Input.GetButtonDown("Start")) {
            if(!isOpen) {
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
        previousSelected = eventSystem.currentSelectedGameObject;
        previousTimeScale = Time.timeScale;

        anim.SetTrigger("Open");
        eventSystem.SetSelectedGameObject(firstSelectable);
        Time.timeScale = 0;
        isOpen = true;
    }

    public void Close() {
        anim.SetTrigger("Close");
        eventSystem.SetSelectedGameObject(previousSelected);
        Time.timeScale = previousTimeScale;
        isOpen = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
