using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

    [Header("Weapon Properties")]
    [SerializeField] private bool autoFire;
    [SerializeField] private float maxFireDuration;
    [SerializeField] private float coolDownDuration;
    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    private Transform bulletsParent;
    public bool isFiring { get; private set; }
    private bool newFireState = false;
    private float fireTimer;
    private float timeMultiplier = -1;
    private bool isCoolDown = false;
    private bool isInput = false;

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if(find != null)
            bulletsParent = find.transform;

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(null, bulletsParent);
        }
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
        float timer = coolDownDuration;
        while(timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }

        isCoolDown = false;
        fireTimer = 0;
    }


}
