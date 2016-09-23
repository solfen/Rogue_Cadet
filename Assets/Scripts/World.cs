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
    public bool hasRoom;
}

public class World : MonoBehaviour {

    public static World instance;

    public List<Zone> zones;
    public Vector2 worldSize;
    public Vector2 backgroundSize;
    public Vector2 roomBaseSize;
    [HideInInspector]
    public Vector2 worldUnitysize;
    public Sector[,] map;
    public bool isNewGame;
    public Dictionary<string, Player> shipTypes = new Dictionary<string, Player>();
    [HideInInspector]
    public List<Enemy> enemies;
    [HideInInspector]
    public Score Score;

    [SerializeField]
    private List<Player> shipTypesList;

    void Awake() {
        instance = this;

        for(int i=0; i< shipTypesList.Count; i++) {
            shipTypes.Add(shipTypesList[i].typeName, shipTypesList[i]);
        }

        string currentType = PlayerPrefs.GetString("selectedShip", "Barbarian");
        Debug.Log(currentType);
        Instantiate(shipTypes[currentType]);

        //roomBaseSize = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, Camera.main.orthographicSize * 2);

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
