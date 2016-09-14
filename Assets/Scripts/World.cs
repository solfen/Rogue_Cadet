using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Zone {
    public string name;
    [Tooltip("In number of backgrounds")]
    public Vector2 position;
    public Vector2 size;
    public GameObject backgroundsPrefab;
}

[System.Serializable]
public class Sector {
    public int zoneType;
}

public class World : MonoBehaviour {

    public static World instance;

    public List<Zone> zones;
    public Vector2 worldSize;
    public Vector2 backgroundSize;
    [HideInInspector]
    public Vector2 worldUnitysize;
    public Sector[,] map;
    public bool isNewGame;

    [HideInInspector]
    public List<Enemy> enemies;

    void Awake() {
        instance = this;

        map = new Sector[(int)worldSize.x, (int)worldSize.y];

        for(int i = 0; i < zones.Count; i++) {
            for(int j = 0; j < zones[i].size.x; j++) {
                for(int k = 0; k < zones[i].size.y; k++) {
                    Sector currentSector = new Sector();
                    currentSector.zoneType = i;

                    int x = ((int)zones[i].position.x) +j;
                    int y = ((int)zones[i].position.y) + k;
                    if(x >= worldSize.x || y >= worldSize.y) {
                        Debug.LogError("ERROR: zone " + zones[i].name + " bigger than world");
                        return;
                    }

                    map[x,y] = currentSector;
                }
            }
        }

        worldUnitysize.Set(worldSize.x * backgroundSize.x, worldSize.y * backgroundSize.y);

        //TODO: check if map has null sectors.
    }

}
