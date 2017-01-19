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
    }

    private void OnPlayerCreated(object playerObj) {
        enabled = true;
        Init(((Player)playerObj).GetComponent<Transform>());
    }

    private void Init(Transform player) {
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if (find != null)
            bulletsParent = find.transform;

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(player, bulletsParent, 1);
        }

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
}
