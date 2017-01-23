using UnityEngine;
using System.Collections;

public class MainMenuPanes : MonoBehaviour {

    private Animator anim;
    private bool isOpen = false;

    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	    if(isOpen && (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Start"))) {
            Close();
        }
	}

    public void Open() {
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        anim.SetTrigger("Open");
        isOpen = true;
    }

    private void Close() {
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
        anim.SetTrigger("Close");
        isOpen = false;
    }
}
