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
        anim.SetTrigger("Open");
        isOpen = true;
    }

    private void Close() {
        anim.SetTrigger("Close");
        isOpen = false;
    }
}
