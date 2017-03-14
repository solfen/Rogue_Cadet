using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponEnemies : MonoBehaviour, ISwitchable {

    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    private Transform player;
    private Transform bulletsParent;
    private bool canSwitch = false;

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.AddEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go == null) {
            EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
            enabled = false;
            return;
        }

        Init(go.GetComponent<Transform>());
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreated);
        EventDispatcher.RemoveEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);
    }

    private void OnPlayerCreated(object playerObj) {
        enabled = true;
        Init(((Player)playerObj).GetComponent<Transform>());
    }

    private void Init(Transform _player) {
        player = _player;
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if (find != null)
            bulletsParent = find.transform;

        InitBulletFountains();

        canSwitch = true;
    }

    private void OnPlayerDeath(object useless) {
        SwitchState(false);
        canSwitch = false;
    }

    public void SwitchState(bool fire) {
        if(canSwitch) {
            for (int i = 0; i < bulletsFountains.Count; i++) {
                bulletsFountains[i].SetFiring(fire);
            }
        }
    }

    private void InitBulletFountains() {
        float dmgMultiplier = PlayerPrefs.GetFloat("EnemiesBulletsDmgMultiplier", 1);
        float fireSpeedMultiplier = PlayerPrefs.GetFloat("EnemiesFireSpeedMultiplier", 1);
        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(player, bulletsParent, dmgMultiplier, fireSpeedMultiplier);
        }
    }

    private void DifficultyChanged(object useless) {
        InitBulletFountains();
    }
}
