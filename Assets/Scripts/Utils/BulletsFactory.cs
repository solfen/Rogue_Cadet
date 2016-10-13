using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletsFactory {

    private static Dictionary<Bullet, Bullet> firstAvailable = new Dictionary<Bullet, Bullet>(4); // dict<prefab, bullets>

    public static void SpawnBullet(Bullet prefab, Vector3 pos, Transform parent, float angle, float dmgMultiplier, Transform target = null) {
        if(!firstAvailable.ContainsKey(prefab)) {
            firstAvailable.Add(prefab, null);
        }

        if(firstAvailable[prefab] == null) {
            firstAvailable[prefab] = GameObject.Instantiate(prefab);
            firstAvailable[prefab].gameObject.SetActive(false);
        }

        firstAvailable[prefab].Init(prefab, parent, pos, angle, dmgMultiplier, target);
        firstAvailable[prefab] = firstAvailable[prefab].nextAvailable;
    }

    public static void BulletDeath(Bullet prefab, Bullet bullet) {
        bullet.nextAvailable = firstAvailable[prefab];
        firstAvailable[prefab] = bullet;
    }
}
