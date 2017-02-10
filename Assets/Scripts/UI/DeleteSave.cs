﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeleteSave : MonoBehaviour {

    private int timePressed = -1;
    [SerializeField] private int nbToPress = 3;
    [SerializeField] private Text nbToPressText;

	// Use this for initialization
	void Start () {
        OnPressed();
    }
	
    public void OnPressed() {
        timePressed++;

        if(timePressed == nbToPress) {
            FileSaveLoad.Delete();
            if(SceneManager.GetActiveScene().buildIndex != 0) {
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
            }
        }

        int pressLeft = nbToPress - timePressed;
        nbToPressText.text = pressLeft <= 0 ? "(Deleted)" : "(Press " + pressLeft + " more time(s))";
    }
}
