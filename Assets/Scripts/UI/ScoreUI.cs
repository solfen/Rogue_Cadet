using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;

    void Awake () {
        EventDispatcher.AddEventListener(Events.SCORE_CHANGED, UpdateScore);
        EventDispatcher.AddEventListener(Events.COMBO_CHANGED, UpdateCombo);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.SCORE_CHANGED, UpdateScore);
        EventDispatcher.RemoveEventListener(Events.COMBO_CHANGED, UpdateCombo);
    }

    public void UpdateScore(object score) {
        scoreText.text = "Money: " + ((int)(float)score) + " $";
    }

    public void UpdateCombo(object combo) {
        comboText.text = "x" + ((float)combo);
    }
}
