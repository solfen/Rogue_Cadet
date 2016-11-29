using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public float maxCombo;
    public float comboDownInterval = 1;

    [SerializeField] private GameData gameData;

    public float score = 0;

    private float combo = 1;
    private float comboDownTimer = 0;
    private float shipGoldPercent;

    // Use this for initialization
    void Start () {
        comboDownTimer = comboDownInterval;
        EventDispatcher.AddEventListener(Events.COLLECTIBLE_TAKEN, CollectibleTaken);
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, KilledEnemy);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.AddEventListener(Events.PLAYER_HIT, PlayerHit);

        shipGoldPercent = gameData.ships[PlayerPrefs.GetInt("SelectedShip", 0)].goldPercent;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.COLLECTIBLE_TAKEN, CollectibleTaken);
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, KilledEnemy);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.RemoveEventListener(Events.PLAYER_HIT, PlayerHit);

    }

    void Update () {
        comboDownTimer -= Time.deltaTime;
        if(comboDownTimer <= 0) {
            combo = Mathf.Max(1, combo - 1);
            comboDownTimer = comboDownInterval;

            ScoreUI.instance.UpdateCombo(combo);
        }
    }

    public void KilledEnemy(object enemy) {
        float scoreToAdd = ((Enemy)enemy).score;

        score += scoreToAdd * combo * shipGoldPercent;
        combo = Mathf.Min(maxCombo, combo+1);
        comboDownTimer = comboDownInterval;

        ScoreUI.instance.UpdateScore(score);
        ScoreUI.instance.UpdateCombo(combo);
    }

    public void PlayerHit(object useless) {
        combo = Mathf.Max(1, combo - 1);

        comboDownTimer = comboDownInterval;
        ScoreUI.instance.UpdateCombo(combo);
    }

    public void CollectibleTaken(object collectible) {
        float value = ((Collectible)collectible).value;

        score += value * combo * shipGoldPercent;
        ScoreUI.instance.UpdateScore(score);
    }

    public void OnPlayerDeath(object useless) {
        float highscore = Mathf.Max(PlayerPrefs.GetFloat("HighScore", 0), score);

        PlayerPrefs.SetFloat("HighScore", highscore);
        PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money", 0) + score);

        DeathScreen.instance.OnPlayerDeath(score, highscore);
    }
}
