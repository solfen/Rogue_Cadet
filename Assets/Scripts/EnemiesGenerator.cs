using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyInstantiation {
    public GameObject Enemy;
    public Vector2 position;
}

[System.Serializable]
public class EnemyPack {
    public string name;
    public List<EnemyInstantiation> enemies = new List<EnemyInstantiation>();
    public int zone;
    public float probabilityMultiplier = 1f;
}

public class EnemiesGenerator : MonoBehaviour {

    public World world;
    [Range(0,1)]
    public float probabilityEmptySector = 0.25f;
    public List<EnemyPack> enemiesPacks;
    public Vector2 startPos;

    private Dictionary<int, List<EnemyPack>> zonesEnemyPacks = new Dictionary<int, List<EnemyPack>>();

	void Start () {
	    for(int i = 0; i < world.zones.Count; i++) {
            zonesEnemyPacks.Add(i, new List<EnemyPack>());
        }

        for(int i = 0; i < enemiesPacks.Count; i++) {
            zonesEnemyPacks[enemiesPacks[i].zone].Add(enemiesPacks[i]);
        }

        for(int x = 0; x < world.worldSize.x; x++) { // starts at 1,1 so that ther's no enemy in the starting zone
            for(int y = 0; y < world.worldSize.y; y++) {
                if(Random.value < probabilityEmptySector || (x == startPos.x && y == startPos.y)) {
                    continue;
                }

                List<int> packsToCheck = new List<int>();
                List<EnemyPack> packs = zonesEnemyPacks[world.map[x, y].zoneType];
                packsToCheck.AddRange(System.Linq.Enumerable.Range(0, packs.Count)); // so that we have a list like [0,1,2,3 ... count-1]
                float baseProba = 1 / (float)packs.Count;

                while (packsToCheck.Count > 0) {
                    int index = packsToCheck[Random.Range(0, packsToCheck.Count)];

                    Debug.Log(baseProba);
                    if (Random.value < packs[index].probabilityMultiplier * baseProba) {
                        InstantiatePack(packs[index].enemies, x, y);
                        break;
                    }

                    packsToCheck.Remove(index);
                }
            }
        }
	}

    private void InstantiatePack(List<EnemyInstantiation> enemies, int x, int y) {
        for(int i = 0; i < enemies.Count; i++) {
            Instantiate(enemies[i].Enemy, new Vector3(enemies[i].position.x + x * world.backgroundSize.x, enemies[i].position.y + y * world.backgroundSize.y, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
