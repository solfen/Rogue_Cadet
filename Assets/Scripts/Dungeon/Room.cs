using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exit {
    public Vector2 pos;
    public Vector2 dir;
}

[System.Serializable]
public class EnemyInstantiation {
    public GameObject Enemy;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public class EnemyPack {
    public string name;
    public List<EnemyInstantiation> enemies = new List<EnemyInstantiation>();
    public float probabilityMultiplier = 1f;
}

public class Room : MonoBehaviour {

    public List<Exit> exits;
    public Vector2 size;
    public List<EnemyPack> possibleEnemies;
    public Transform enemiesParent;
    public Transform bulletsParent;

    [HideInInspector]
    public Vector2 pos = new Vector2();
    
    private GameObject enemy;

    void Start() {
        if (possibleEnemies.Count > 0) {
            List<EnemyInstantiation> enemies = possibleEnemies[Random.Range(0, possibleEnemies.Count)].enemies;
            for (int i = 0; i < enemies.Count; i++) {
                enemy = Instantiate(enemies[i].Enemy, enemiesParent, false) as GameObject;
                enemy.transform.localPosition = enemies[i].position;
                enemy.transform.localRotation = enemies[i].rotation;

                Weapon weapon = enemy.GetComponent<Weapon>();
                if(weapon != null) {
                    weapon.bulletsParent = bulletsParent;
                }
            }
        }
    }
}

