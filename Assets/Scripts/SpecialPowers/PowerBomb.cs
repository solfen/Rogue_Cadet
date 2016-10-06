using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBomb : MonoBehaviour, ISpecialPower {

    public static PowerBomb instance;

    public float damage;
    public bool destroyBullets = true;
    public Animator explosion;

    private Room currentRoom;

    void Awake() {
        instance = this;
    }

    public void Activate() {
        if (destroyBullets) {
            int bulletsCount = currentRoom.bulletsParent.childCount;
            for (int i = bulletsCount - 1; i >= 0; i--) {
                GameObject.Destroy(currentRoom.bulletsParent.GetChild(i).gameObject);
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
