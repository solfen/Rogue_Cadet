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
        bulletsParent = GameObject.FindGameObjectWithTag("BulletsContainer").transform;
        if(bulletsParent != null) {
            Debug.LogError("No bullet parent! Bomb can't work");
        }
    }

    public void Activate() {
        if (destroyBullets && bulletsParent != null) {
            for (int i = bulletsParent.childCount - 1; i >= 0; i--) {
                bulletsParent.GetChild(i).GetComponent<Bullet>().BombKill();
            }
        }

        int enemiesCount = currentRoom.enemiesParent.childCount;
        for (int i = enemiesCount - 1; i >= 0; i--) {
            currentRoom.enemiesParent.GetChild(i).GetComponent<Enemy>().Hit(damage);
        }

        explosion.SetTrigger("Explode");
    }

    public void OnPlayerEnterRoom(GraphRoom room) {
        currentRoom = room.roomInstance;
    }
}
