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
    private float fireTimer = 0;

    void Awake() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        enabled = false;
    }

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
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
        fireTimer += Time.deltaTime;

        newFireState = (autoFire || (useController && Input.GetButton("MainShot"))) && ( (isFiring && fireTimer < maxFireDuration) || (!isFiring && fireTimer > coolDownDuration));

        if(isFiring != newFireState) {
            fireTimer = 0;
            isFiring = newFireState;
            SetFiring(isFiring);
        }
    }

    public void SetFiring(bool fire) {
        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].SetFiring(fire);
        }
    }
	
}
