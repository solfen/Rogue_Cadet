using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBomb : MonoBehaviour, ISpecialPower {

    public static PowerBomb instance;

    public float damage;
    public bool destroyBullets = true;
    public Animator explosion;

    private Transform bulletsParent;
    private Room currentRoom;

    void Awake() {
        instance = this;
    }

    void Start() {
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if(find == null) {
            Debug.LogError("No bullet parent! Bomb can't work");
            return;
        }

        bulletsParent = find.transform;
    }

    public void Activate() {
        if (destroyBullets && bulletsParent != null) {
            Transform child = null;
            for (int i = bulletsParent.childCount - 1; i >= 0; i--) {
                child = bulletsParent.GetChild(i);
                if(child.gameObject.activeSelf)
                    child.GetComponent<Bullet>().BombKill();
            }
        }

        Transform enemiesParent = currentRoom.enemiesParent.GetChild(0);
        Enemy enemy = null;
        int enemiesCount = enemiesParent.childCount;
        for (int i = enemiesCount - 1; i >= 0; i--) {
            enemy = enemiesParent.GetChild(i).GetComponent<Enemy>();
            if(enemy != null)
                enemy.Hit(damage);
        }

        explosion.SetTrigger("Explode");
    }

    public void OnPlayerEnterRoom(GraphRoom room) {
        currentRoom = room.roomInstance;
    }
}
