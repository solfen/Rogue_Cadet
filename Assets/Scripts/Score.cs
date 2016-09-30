using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public float maxCombo;
    public float comboDownInterval = 1;

    public float score = 0;
    private float combo = 1;
    private float comboDownTimer = 0;

    // Use this for initialization
    void Start () {
        World.instance.Score = this;
        comboDownTimer = comboDownInterval;
    }

    void Update () {
        comboDownTimer -= Time.deltaTime;
        if(comboDownTimer <= 0) {
            combo = Mathf.Max(1, combo - 1);
            comboDownTimer = comboDownInterval;
            ScoreUI.instance.UpdateCombo(combo);
            Debug.Log("Inactive! combo down: " + combo);

        }
    }

    public void KilledEnemy(Enemy enemy) {
        score += enemy.score * combo;
        combo = Mathf.Min(maxCombo, combo+1);
        Debug.Log("Kill! combo raise: " + combo);
        comboDownTimer = comboDownInterval;
        ScoreUI.instance.UpdateScore(score);
        ScoreUI.instance.UpdateCombo(combo);
    }

    public void PlayerHit() {
        combo = Mathf.Max(1, combo - 1);
        Debug.Log("Hit! combo down: " + combo);

        comboDownTimer = comboDownInterval;
        ScoreUI.instance.UpdateCombo(combo);
    }

    public void CollectibleTaken(float value) {
        score += value * combo;
        ScoreUI.instance.UpdateScore(score);
    }
}
