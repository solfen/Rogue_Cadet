﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public float maxCombo;
    public float comboDownInterval = 1;

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

        ShipConfig shipConfig = GlobalData.instance.gameData.ships[GlobalData.instance.saveData.selectedShip];
        shipGoldPercent = shipConfig.goldPercent + GlobalData.instance.saveData.goldUpgradeNb * shipConfig.goldUpgradeRaise;
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

    public void KilledEnemy(object enemy) {
        float scoreToAdd = ((Enemy)enemy).score;

        score += scoreToAdd * combo * shipGoldPercent;
        combo = Mathf.Min(maxCombo, combo+1);
        comboDownTimer = comboDownInterval;

        EventDispatcher.DispatchEvent(Events.SCORE_CHANGED, score);
        EventDispatcher.DispatchEvent(Events.COMBO_CHANGED, combo);
    }

    public void PlayerHit(object useless) {
        combo = 1;

        comboDownTimer = comboDownInterval;
        EventDispatcher.DispatchEvent(Events.COMBO_CHANGED, combo);
    }

    public void CollectibleTaken(object collectible) {
        float value = ((Collectible)collectible).value;

        score += value * combo * shipGoldPercent;
        EventDispatcher.DispatchEvent(Events.SCORE_CHANGED, score);
    }

    public void OnPlayerDeath(object useless) {
        SaveData data = FileSaveLoad.Load();
        data.money += score;
        data.highScore = Mathf.Max(data.highScore, score);
        FileSaveLoad.Save(data);
    }
}
