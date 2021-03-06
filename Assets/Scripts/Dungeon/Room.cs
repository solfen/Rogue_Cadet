﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exit {
    public Vector2 pos;
    public Vector2 dir;

    public static bool AreExitsConnected(Exit exit, Exit adjacent, GraphRoom room, GraphRoom adjRoom) {
        return ((adjacent.dir.x != 0 && adjacent.dir.x == -exit.dir.x && adjRoom.pos.y + adjacent.pos.y == room.pos.y + exit.pos.y) 
        || (adjacent.dir.y != 0 && adjacent.dir.y == -exit.dir.y && adjRoom.pos.x + adjacent.pos.x == room.pos.x + exit.pos.x));
    }
}

[System.Serializable]
public class EnemyPack {
    public string name;
    public GameObject container;
}

[System.Serializable]
public enum RoomType {
    NORMAL,
    BOSS,
    TREASURE,
    SPECIAL
}

public class Room : MonoBehaviour {

    public bool debug = false;
    public Transform enemiesParent;
    [Header("Dungeon data")]
    public int zoneIndex;
    public RoomType type = RoomType.NORMAL;
    public Vector2 size;
    public List<Exit> exits = new List<Exit>();
    [Header("Enemies configuration")]
    [Tooltip("One container is selected at random at start")]
    public List<EnemyPack> enemiesContainers;

    [HideInInspector]
    public Vector2 pos = new Vector2();

    void Start() {
        if (debug)
            return;

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

