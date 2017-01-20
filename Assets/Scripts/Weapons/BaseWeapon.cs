﻿using UnityEngine;
using System.Collections;

public abstract class BaseWeapon : MonoBehaviour {
    [Header("Weapon Properties")]
    public string displayName;
    public float maxFireDuration;
    [HideInInspector] public float coolDownTimer;

    public bool isFiring { get; protected set; }
    public float fireTimer { get; protected set; }

    [SerializeField] private float coolDownDuration;
    protected bool isCoolDown;
    protected float damageInfluencer;

    void Awake() {
        enabled = false;
    }

    protected virtual void Start() {
        ShipConfig shipConfig = GlobalData.instance.gameData.ships[GlobalData.instance.saveData.selectedShip];
        damageInfluencer = shipConfig.damagePrecent + GlobalData.instance.saveData.damageUpgradeNb * shipConfig.damageUpgradeRaise;
    }

    public void Activate() {
        enabled = true;
        EventDispatcher.DispatchEvent(Events.WEAPON_READY, this);
    }

    public void Disable() {
        SetFiring(false);
        enabled = false;
    }

    protected abstract void SetFiring(bool fireState);

    protected virtual void Update() {
        fireTimer -= Time.deltaTime;
        if (!isCoolDown && fireTimer >= maxFireDuration) {
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }

        bool isInput = Input.GetButton("MainShot") || Input.GetAxis("MainShot") < -0.5f;
        bool newFireState = !isCoolDown && isInput && fireTimer < maxFireDuration;

        if (isFiring != newFireState) {
            fireTimer = Mathf.Max(0, fireTimer);
            isFiring = newFireState;
            SetFiring(isFiring);
        }
    }

    protected IEnumerator CoolDown() {
        EventDispatcher.DispatchEvent(Events.WEAPON_COOLDOWN_START, null);
        coolDownTimer = coolDownDuration;

        while (coolDownTimer > 0) {
            coolDownTimer -= Time.deltaTime;
            yield return null;
        }

        isCoolDown = false;
        fireTimer = 0;
        EventDispatcher.DispatchEvent(Events.WEAPON_COOLDOWN_END, null);
    }
}