using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

    [Header("Weapon Properties")]
    [SerializeField] private bool autoFire;
    public float maxFireDuration;
    [SerializeField] private float coolDownDuration;
    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    public bool isFiring { get; private set; }
    public float fireTimer { get; private set; }
    public bool isCoolDown { get; private set; }

    private Transform bulletsParent;
    private bool newFireState = false;
    private float timeMultiplier = -1;
    private bool isInput = false;

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if(find != null)
            bulletsParent = find.transform;

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(null, bulletsParent);
        }

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
        fireTimer += Time.deltaTime * timeMultiplier;
        if(!isCoolDown && fireTimer >= maxFireDuration) {
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }

        isInput = autoFire || Input.GetButton("MainShot") || Input.GetAxis("MainShot") < -0.5f;
        newFireState = !isCoolDown && isInput && fireTimer < maxFireDuration;

        if(isFiring != newFireState) {
            fireTimer = Mathf.Max(0, fireTimer);
            timeMultiplier *= -1;

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
        float timer = coolDownDuration;

        while(timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }

        isCoolDown = false;
        fireTimer = 0;
        EventDispatcher.DispatchEvent(Events.WEAPON_COOLDOWN_END, null);
    }


}
