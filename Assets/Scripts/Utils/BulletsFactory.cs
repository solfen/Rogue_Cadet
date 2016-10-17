using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletsFactory {

    private static Dictionary<Bullet, Bullet> firstAvailable = new Dictionary<Bullet, Bullet>(4); // dict<prefab, bullet>

    public static void SpawnBullet(BulletStats bulletstats, Vector3 pos, Vector2 dir) {
        Bullet prefab = bulletstats.prefab;

        if (!firstAvailable.ContainsKey(prefab)) {
            firstAvailable.Add(prefab, null);
        }

        if(firstAvailable[prefab] == null) {
            firstAvailable[prefab] = GameObject.Instantiate(prefab);
            firstAvailable[prefab].gameObject.SetActive(false);
        }

        firstAvailable[prefab].Init(bulletstats, pos, dir);
        firstAvailable[prefab] = firstAvailable[prefab].nextAvailable;
    }

    public static void BulletDeath(Bullet prefab, Bullet bullet) {
        bullet.nextAvailable = firstAvailable[prefab];
        firstAvailable[prefab] = bullet;
    }
}
