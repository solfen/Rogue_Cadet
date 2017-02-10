using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public int maxStock;
    public int currentStock;

    [SerializeField] private Animator explosion;
    [SerializeField] private float baseDamage = 50; // tmp needs to be in global config and specific to each ship. But I'm too close to the deadline to balance it.
    [SerializeField] private float damageRadius = 10;
    [SerializeField] private LayerMask layerMask;

    private Transform _transform;
    private float damage;

    void Start() {
        _transform = GetComponent<Transform>();

        SaveData saveData = GlobalData.instance.saveData;
        ShipConfig shipconfig = GlobalData.instance.gameData.ships[saveData.selectedShip];
        maxStock = saveData.bombUpgradeNb * shipconfig.bombStockUpgradeRaise;
        currentStock = maxStock;
        damage = baseDamage * (1 + saveData.bombDamageUpgradeNb * shipconfig.bombDamagePerUpgrade);

        EventDispatcher.DispatchEvent(Events.BOMB_USED, this); //init UI;
    }

    void Update() {
        if(Time.timeScale != 0 && InputManager.GetButtonDown(InputManager.GameButtonID.BOMB) && currentStock > 0) {
            Activate();
        }
    }

    private void Activate() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, damageRadius, layerMask.value);
        for (int i = 0; i < colliders.Length; i++) {
            Enemy enemy = colliders[i].GetComponent<Enemy>();
            if(enemy != null) {
                enemy.Hit(damage);
            }
            else {
                colliders[i].GetComponent<Bullet>().BombKill();
            }
        }

        explosion.SetTrigger("Explode");
        currentStock--;
        EventDispatcher.DispatchEvent(Events.BOMB_USED, this);
    }
}
