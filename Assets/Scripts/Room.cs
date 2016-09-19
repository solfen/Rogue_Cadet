using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exit {
    public Vector2 pos;
    public Vector2 dir;
    public bool connected = false;
}

public class Room : MonoBehaviour {

    public List<Exit> exits;
    public Vector2 size;
    public Vector2 pos = new Vector2();
    //public List<EnemyPack> possibleEnemies;
}
