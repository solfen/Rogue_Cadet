using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletWeapon : BaseWeapon {

    [SerializeField] private float timePerShot;

    [Header("Fountains")]
    [SerializeField] private List<BulletFountain> bulletsFountains = new List<BulletFountain>();

    private Transform bulletsParent;

    protected override void Start() {
        base.Start();

        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);

        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if(find != null)
            bulletsParent = find.transform;

        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].Init(null, bulletsParent, damageInfluencer);
        }

        bulletsFountains[0].OnFire += OnFountainFire;
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    private void OnPlayerDeath(object useless) {
        Disable();
    }

    protected override void SetFiring(bool fire) {
        for (int i = 0; i < bulletsFountains.Count; i++) {
            bulletsFountains[i].SetFiring(fire);
        }
    }

    private void OnFountainFire() {
        fireTimer += timePerShot;
    }
}
