using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public int zone;
    public float probabilityMultiplier = 1f;
}

[System.Serializable]
public class BossPack {
    public GameObject boss;
    public int zone;
    [HideInInspector]
    public float proba = 0;
    public bool canPop = true;
}

public class EnemiesGenerator : MonoBehaviour {

    [Range(0,1)]
    public float probabilityEmptySector = 0.25f;
    public List<EnemyPack> enemiesPacks;
    public List<BossPack> bosses;
    public Vector2 startPos;
    public Transform enemiesParent;
    public Transform bossTransform;

    private World world;
    private Dictionary<int, List<EnemyPack>> zonesEnemyPacks = new Dictionary<int, List<EnemyPack>>();
    private Dictionary<int, BossPack> zonesBoss = new Dictionary<int, BossPack>();
    private float bossProbability;

    void Start () {
        world = World.instance;

        for (int i = 0; i < world.zones.Count; i++) {
            zonesEnemyPacks.Add(i, new List<EnemyPack>());
        }

        for(int i = 0; i < enemiesPacks.Count; i++) {
            zonesEnemyPacks[enemiesPacks[i].zone].Add(enemiesPacks[i]);
        }

        for (int i = 0; i < bosses.Count; i++) {
            if (world.isNewGame) {
                PlayerPrefs.SetInt("Defeated_Boss" + i, 0);
            }

            bosses[i].canPop = PlayerPrefs.GetInt("Defeated_Boss" + i) == 0;
            zonesBoss[bosses[i].zone] = bosses[i];

        }

        for (int x = 0; x < world.worldSize.x; x++) { // starts at 1,1 so that ther's no enemy in the starting zone
            for (int y = 0; y < world.worldSize.y; y++) {
                int zoneIndex = world.map[x, y].zoneType;
                BossPack boss = zonesBoss[zoneIndex];

                if(boss.canPop) {
                    boss.proba += 1 / (world.zones[zoneIndex].size.x * world.zones[zoneIndex].size.y);
                    if (Random.value < boss.proba) {
                        Instantiate(boss.boss, new Vector3(boss.boss.transform.position.x + x * world.backgroundSize.x, boss.boss.transform.position.y + y * world.backgroundSize.y, 0), Quaternion.identity, bossTransform);
                        boss.canPop = false;
                        continue;
                    }
                }

                if(Random.value < probabilityEmptySector || (x == startPos.x && y == startPos.y)) {
                    continue;
                }

                List<int> packsToCheck = new List<int>();
                List<EnemyPack> packs = zonesEnemyPacks[zoneIndex];
                packsToCheck.AddRange(System.Linq.Enumerable.Range(0, packs.Count)); // so that we have a list like [0,1,2,3 ... count-1]
                float baseProba = 1 / (float)packs.Count;

                while (packsToCheck.Count > 0) {
                    int index = packsToCheck[Random.Range(0, packsToCheck.Count)];

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
            Instantiate(enemies[i].Enemy, new Vector3(enemies[i].position.x + x * world.backgroundSize.x, enemies[i].position.y + y * world.backgroundSize.y, 0), Quaternion.identity, enemiesParent);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
