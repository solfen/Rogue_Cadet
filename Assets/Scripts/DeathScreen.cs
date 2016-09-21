using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour {

    public static DeathScreen instance;

    [SerializeField]
    private int levelToLoad = 1;
    private Animator anim;

    void Awake() {
        instance = this;
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    void Update () {
        if(Input.GetButtonDown("Restart")) {
            Application.LoadLevel(levelToLoad);
        }
    }

    public void OnPlayerDeath() {
        anim.enabled = true;
        enabled = true;
    }
}
