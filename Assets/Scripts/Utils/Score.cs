using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public float maxCombo;
    public float comboDownInterval = 1;

    public float score = 0;

    private float combo = 1;
    private float comboDownTimer = 0;
    private float shipGoldPercent;
    private float difficultyGoldMultiplier;

    // Use this for initialization
    void Start () {
        comboDownTimer = comboDownInterval;
        EventDispatcher.AddEventListener(Events.COLLECTIBLE_TAKEN, CollectibleTaken);
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, KilledEnemy);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.AddEventListener(Events.PLAYER_HIT, PlayerHit);
        EventDispatcher.AddEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);

        ShipConfig shipConfig = GlobalData.instance.gameData.ships[GlobalData.instance.saveData.selectedShip];
        shipGoldPercent = shipConfig.goldPercent + GlobalData.instance.saveData.goldUpgradeNb * shipConfig.goldUpgradeRaise;
        difficultyGoldMultiplier = PlayerPrefs.GetFloat("GoldMultiplier", 1);
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

            EventDispatcher.DispatchEvent(Events.COMBO_CHANGED, combo);
        }
    }

    private void KilledEnemy(object enemy) {
        float scoreToAdd = ((Enemy)enemy).score;

        score += scoreToAdd * combo * shipGoldPercent * difficultyGoldMultiplier;
        combo = Mathf.Min(maxCombo, combo+1);
        comboDownTimer = comboDownInterval;

        EventDispatcher.DispatchEvent(Events.SCORE_CHANGED, score);
        EventDispatcher.DispatchEvent(Events.COMBO_CHANGED, combo);
    }

    private void PlayerHit(object useless) {
        combo = 1;

        comboDownTimer = comboDownInterval;
        EventDispatcher.DispatchEvent(Events.COMBO_CHANGED, combo);
    }

    private void CollectibleTaken(object collectible) {
        float value = ((Collectible)collectible).value;

        score += value * combo * shipGoldPercent * difficultyGoldMultiplier;
        EventDispatcher.DispatchEvent(Events.SCORE_CHANGED, score);
    }

    private void OnPlayerDeath(object useless) {
        SaveData data = FileSaveLoad.Load();
        data.money += score;
        data.highScore = Mathf.Max(data.highScore, score);
        FileSaveLoad.Save(data);
    }

    private void DifficultyChanged(object useless) {
        difficultyGoldMultiplier = PlayerPrefs.GetFloat("GoldMultiplier", 1);
    }
}
