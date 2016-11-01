using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponEnemies : MonoBehaviour, ISwitchable {

    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    private Transform player;
    private Transform bulletsParent;
    private bool canSwitch = false;

    void Awake() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
    }

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if (find != null)
            bulletsParent = find.transform;
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    private void OnPlayerCreation(object _player) {
        player = ((Player)_player).GetComponent<Transform>();

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(player, bulletsParent);
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
