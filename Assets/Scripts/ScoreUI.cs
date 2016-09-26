﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    public static ScoreUI instance;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text comboText;

    void Awake () {
        instance = this;
    }

    public void UpdateScore(float score) {
        scoreText.text = "Score: " + score;
    }

    public void UpdateCombo(float combo) {
        comboText.text = "Combo: " + "x" + combo;
    }
}