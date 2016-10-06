using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DungeonGeneratorRoomsFormatTool : MonoBehaviour {

    public Dungeon generator;
    public int zone;
    public bool isDeadEnd;
    public List<Room> rooms;

    private ZoneRooms list;

    // Update is called once per frame
    void Update() {
        list = isDeadEnd ? generator.deadEndRooms[zone] : generator.zoneRooms[zone];

        if (EditorUtility.DisplayDialog("Add new room list ?", "You sure? Have you check all the variables?", "Yeah, yeah shut up.", "I'm so stupid! Thanks!")) {
            for (int i = 0; i < rooms.Count; i++) {
                RoomList roomPack = new RoomList();
                roomPack.rooms.Add(rooms[i]);
                list.roomList.Add(roomPack);
            }

        }

        rooms = new List<Room>();
        enabled = false;
    }
}
