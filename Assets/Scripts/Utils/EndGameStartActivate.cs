using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameStartActivate : MonoBehaviour {

    [SerializeField] private float timeBeforeActive;
    [SerializeField] private LoadSceneAsync loader;

    private float timer = 0;
	// Update is called once per frame
	void Update () {
        if (timer >= timeBeforeActive && Input.GetButtonDown("Start")) {
            
            loader.Activate();
        }

        timer += Time.unscaledDeltaTime;
    }
}
