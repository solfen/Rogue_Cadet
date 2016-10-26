using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

    public Transform bulletsParent;

    [Header("Weapon Properties")]
    [SerializeField] private bool autoFire;
    [SerializeField] private bool useController = false;
    [SerializeField] private float maxFireDuration;
    [SerializeField] private float coolDownDuration;
    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    private Transform player;
    private float damageMultiplier = 1;
    public bool isFiring { get; private set; }
    private bool newFireState = false;
    private float fireTimer;
    private float timeMultiplier = -1;
    private bool isCoolDown = false;

    void Awake() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        enabled = false;
    }

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        bulletsParent = bulletsParent != null ? bulletsParent : GameObject.FindGameObjectWithTag("BulletsContainer").transform;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    private void OnPlayerCreation(object _player) {
        player = ((Player)_player).GetComponent<Transform>();

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(player, bulletsParent, damageMultiplier);
        }

        enabled = true;
    }

    private void OnPlayerDeath(object useless) {
        enabled = false;
    }
	
    void Update () {
        fireTimer += Time.deltaTime * timeMultiplier;
        if(!isCoolDown && fireTimer >= maxFireDuration) {
            isCoolDown = true;
            StartCoroutine(CoolDown());
        }

        newFireState = !isCoolDown && (autoFire || (useController && Input.GetButton("MainShot"))) && fireTimer < maxFireDuration;

        if(isFiring != newFireState) {
            fireTimer = Mathf.Max(0, fireTimer);
            timeMultiplier *= -1;

            isFiring = newFireState;
            SetFiring(isFiring);
        }
    }

    public void SetFiring(bool fire) {
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
