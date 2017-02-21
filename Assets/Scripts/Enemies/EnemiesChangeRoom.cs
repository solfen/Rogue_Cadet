using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesChangeRoom : MonoBehaviour {

    private Transform _transform;
    private Dungeon dungeon;
    private GraphRoom currentRoom;

    // Use this for initialization
    void Start () {
        _transform = GetComponent<Transform>();

        GameObject go = GameObject.FindGameObjectWithTag("Dungeon");
        if (go != null) {
            dungeon = go.GetComponent<Dungeon>();
        }
    }
	
    void FixedUpdate() {
        if (dungeon != null) {
            GraphRoom newRoom = dungeon.GetRoomFromPosition(_transform.position);
            if (newRoom != null && newRoom != currentRoom) {
                currentRoom = newRoom;

                Transform enemiesParent = newRoom.roomInstance.enemiesParent;
                if (enemiesParent != null && enemiesParent.childCount > 0)
                    _transform.parent = newRoom.roomInstance.enemiesParent.GetChild(0);
            }
        }
    }
}
