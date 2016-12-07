using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
    [Header("Weapon Properties")]
    public string displayName;
    public float maxFireDuration;
    [SerializeField] private float coolDownDuration;
    [SerializeField] private float timePerShot;
    [SerializeField] private bool autoFire;

    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    [HideInInspector] public float coolDownTimer;
    public bool isFiring { get; private set; }
    public float fireTimer { get; private set; }
    public bool isCoolDown { get; private set; }


    private Transform bulletsParent;
    private bool newFireState = false;
    private bool isInput = false;

    void Awake() {
        enabled = false;
    }

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

        bulletsFountains[0].OnFire += OnFountainFire;
    }

    public void Activate() {
        enabled = true;
        EventDispatcher.DispatchEvent(Events.WEAPON_READY, this);
    }

    public void Disable() {
        SetFiring(false);
        enabled = false;
    }

    private void OnPlayerDeath(object useless) {
        Disable();
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

    private void OnFountainFire() {
        fireTimer += timePerShot;
    }


}
