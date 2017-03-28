using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuPanes : MonoBehaviour {

    [SerializeField] private MainMenuPanes mainMenuPaneParent;
    [SerializeField] private GameObject firstSelectable = null;

    private Animator anim;
    private GameObject lastSelected;
    public bool isOpen { get; private set; }

    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(isOpen && (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Pause"))) {
            Close();
        }
	}

    public void Open() {
        lastSelected = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(firstSelectable);
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        anim.SetTrigger("Open");
        isOpen = true;

        if (mainMenuPaneParent != null) {
            mainMenuPaneParent.enabled = false;
        }
    }

    public void Close() {
        EventSystem.current.SetSelectedGameObject(lastSelected);
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
        anim.SetTrigger("Close");
        isOpen = false;

        if (mainMenuPaneParent != null) {
            mainMenuPaneParent.enabled = true;
            if (Input.GetButtonDown("Start"))
                mainMenuPaneParent.Close();
        }
    }
}
