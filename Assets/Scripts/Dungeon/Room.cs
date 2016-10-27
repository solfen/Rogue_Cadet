using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exit {
    public Vector2 pos;
    public Vector2 dir;
}

[System.Serializable]
public class EnemyPack {
    public string name;
    public GameObject container;
}

public class Room : MonoBehaviour {

    public Transform enemiesParent;
    [Header("Dungeon data")]
    public int zoneIndex;
    public Vector2 size;
    public List<Exit> exits = new List<Exit>();
    [Header("Enemies configuration")]
    [Tooltip("One container is selected at random at start")]
    public List<EnemyPack> enemiesContainers;

    [HideInInspector]
    public Vector2 pos = new Vector2();

    void Start() {
        if (enemiesContainers.Count > 0) {
            GameObject selectedContainer = enemiesContainers[Random.Range(0, enemiesContainers.Count)].container;
            selectedContainer.SetActive(true);

            for (int i = enemiesContainers.Count-1; i >= 0; i--) {
                if(enemiesContainers[i].container != selectedContainer) {
                    Destroy(enemiesContainers[i].container);
                }
            }
        }
    }
}

