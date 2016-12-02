using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public int maxStock;
    public int currentStock;

    [SerializeField] private float damage;
    [SerializeField] private Animator explosion;

    private Transform bulletsParent;
    private Room currentRoom;

    void Start() {
        GameObject find = GameObject.FindGameObjectWithTag("BulletsContainer");
        if(find == null) {
            Debug.LogError("No bullet parent! Bomb can't work");
            return;
        }
        bulletsParent = find.transform;

        SaveData saveData = GlobalData.instance.saveData;
        ShipConfig shipconfig = GlobalData.instance.gameData.ships[saveData.selectedShip];
        maxStock = saveData.bombUpgradeNb * shipconfig.bombStockUpgradeRaise;
        currentStock = maxStock;
        damage = saveData.bombDamageUpgradeNb * shipconfig.bombDamagePerUpgrade;

        EventDispatcher.AddEventListener(Events.PLAYER_ENTER_ROOM, OnPlayerEnterRoom);
        EventDispatcher.DispatchEvent(Events.BOMB_USED, this); //init UI;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.PLAYER_ENTER_ROOM, OnPlayerEnterRoom);
    }

    void Update() {
        if(Input.GetButtonDown("Bomb") && currentStock > 0) {
            Activate();
            currentStock--;
        }
    }

    private void Activate() {
        if (bulletsParent != null) {
            Transform child = null;
            for (int i = bulletsParent.childCount - 1; i >= 0; i--) {
                child = bulletsParent.GetChild(i);
                if(child.gameObject.activeSelf)
                    child.GetComponent<Bullet>().BombKill();
            }
        }

        if(currentRoom.enemiesParent.childCount > 0) {
            Transform enemiesParent = currentRoom.enemiesParent.GetChild(0);
            Enemy enemy = null;
            int enemiesCount = enemiesParent.childCount;
            for (int i = enemiesCount - 1; i >= 0; i--) {
                enemy = enemiesParent.GetChild(i).GetComponent<Enemy>();
                if(enemy != null)
                    enemy.Hit(damage);
            }

            explosion.SetTrigger("Explode");
            EventDispatcher.DispatchEvent(Events.BOMB_USED, this);
        }
    }

    public void OnPlayerEnterRoom(object roomObj) {
        currentRoom = ((GraphRoom)roomObj).roomInstance;
    }
}
