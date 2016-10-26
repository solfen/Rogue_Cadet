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
    public float probabilityMultiplier = 1f;
}

public class Room : MonoBehaviour {

    public List<Exit> exits = new List<Exit>();
    public Vector2 size;
    [Tooltip("One container is selected at random at start")]
    public List<EnemyPack> enemiesContainers;
    public Transform enemiesParent;

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

