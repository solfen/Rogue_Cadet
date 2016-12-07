﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
    [Header("Weapon Properties")]
    [SerializeField] private bool autoFire;
    [SerializeField] private float coolDownDuration;
    [SerializeField] private float timePerShot;
    public float maxFireDuration;

    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    [HideInInspector] public float coolDownTimer;
    public bool isFiring { get; private set; }
    public float fireTimer { get; private set; }
    public bool isCoolDown { get; private set; }


    private Transform bulletsParent;
    private bool newFireState = false;
    private bool isInput = false;

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if(find != null)
            bulletsParent = find.transform;

        ShipConfig shipConfig = GlobalData.instance.gameData.ships[GlobalData.instance.saveData.selectedShip];
        float damageInfluencer = shipConfig.damagePrecent + GlobalData.instance.saveData.damageUpgradeNb * shipConfig.damageUpgradeRaise;

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(null, bulletsParent, damageInfluencer);
        }

        bulletsFountains[0].OnFire += Test;

        EventDispatcher.DispatchEvent(Events.WEAPON_READY, this);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    private void OnPlayerDeath(object useless) {
        enabled = false;
        SetFiring(false);
    }

    void Update () {
        fireTimer -= Time.deltaTime;
        if(!isCoolDown && fireTimer >= maxFireDuration) {
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }

        isInput = autoFire || Input.GetButton("MainShot") || Input.GetAxis("MainShot") < -0.5f;
        newFireState = !isCoolDown && isInput && fireTimer < maxFireDuration;

        if(isFiring != newFireState) {
            fireTimer = Mathf.Max(0, fireTimer);
            isFiring = newFireState;
            SetFiring(isFiring);
        }
    }

    private void SetFiring(bool fire) {
        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].SetFiring(fire);
        }
    }

    IEnumerator CoolDown() {
        EventDispatcher.DispatchEvent(Events.WEAPON_COOLDOWN_START, null);
        coolDownTimer = coolDownDuration;

        while(coolDownTimer > 0) {
            coolDownTimer -= Time.deltaTime;
            yield return null;
        }

        isCoolDown = false;
        fireTimer = 0;
        EventDispatcher.DispatchEvent(Events.WEAPON_COOLDOWN_END, null);
    }

    private void Test() {
        fireTimer += timePerShot;
    }


}
