using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuPanes : MonoBehaviour {

    private Animator anim;
    private bool isOpen = false;
    private GameObject lastSelected;

    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(isOpen && !InputManager.isRebinding && (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Start"))) {
            Close();
        }
	}

    public void Open() {
        lastSelected = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        anim.SetTrigger("Open");
        isOpen = true;
    }

    private void Close() {
        EventSystem.current.SetSelectedGameObject(lastSelected);
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
        anim.SetTrigger("Close");
        isOpen = false;
    }
}
