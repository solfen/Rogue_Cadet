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
    public bool notBuildableFlag = false;
    public GraphRoom room;
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
    [HideInInspector]
    public List<Enemy> enemies;
    [HideInInspector]
    public Score Score;
    public List<Weapon> weapons;
    public List<GameObject> bombs;
    [HideInInspector]
    public bool useGamedad = false;

    private List<GameObject> activeRooms = new List<GameObject>();

    void Awake() {
        instance = this;
        Time.timeScale = 0;

        if (isNewGame) {
            PlayerPrefs.DeleteAll();
        }

        //roomBaseSize = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, Camera.main.orthographicSize * 2);
        InitMap();
        worldUnitysize.Set(worldSize.x * backgroundSize.x, worldSize.y * backgroundSize.y);

        if(Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "") {
            useGamedad = true;
        }
        //TODO: check if map has null sectors.
    }

    public void InitMap() {
        map = new Sector[(int)worldSize.x, (int)worldSize.y];

        for (int i = 0; i < zones.Count; i++) {
            for (int j = 0; j < zones[i].size.x; j++) {
                for (int k = 0; k < zones[i].size.y; k++) {
                    Sector currentSector = new Sector();
                    currentSector.zoneType = i;

                    int x = ((int)zones[i].position.x) + j;
                    int y = ((int)zones[i].position.y) + k;
                    if (x >= worldSize.x || y >= worldSize.y) {
                        Debug.LogError("ERROR: zone " + zones[i].name + " bigger than world");
                        return;
                    }

                    map[x, y] = currentSector;
                }
            }
        }
    }

    public void OnPlayerOnPlayerEnterRoom(GraphRoom room) {
        List<GameObject> newActiveRooms = new List<GameObject>();

        int endX = (int)(room.pos.x + room.roomPrefab.size.x);
        int endY = (int)(room.pos.y + room.roomPrefab.size.y);
        GameObject testRoom;
        for (int i = (int)room.pos.x-1; i <= endX; i++) {
            for (int j = (int)room.pos.y - 1; j <= endY; j++) {
                if(i >= 0 && i < worldSize.x && j >= 0 && j < worldSize.y && map[i, j].room != null) {
                    testRoom = map[i, j].room.roomInstance.gameObject;
                    
                    if (!activeRooms.Contains(testRoom)) {
                        testRoom.SetActive(true);
                        activeRooms.Add(testRoom);
                    }
                    if(!newActiveRooms.Contains(testRoom)) {
                        newActiveRooms.Add(testRoom);
                    }
                }
            }
        }

        for (int i = activeRooms.Count - 1; i >= 0; i--) {
            if(!newActiveRooms.Contains(activeRooms[i])) {
                activeRooms[i].SetActive(false);
                activeRooms.RemoveAt(i);
            }
        }
    }

}
