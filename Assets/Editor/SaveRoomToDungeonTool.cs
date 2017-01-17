using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveRoomToDungeonTool {

    [MenuItem("Tools/Save room to dungeon _F7", true)]
    private static bool Validate() {
        return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Room>() != null;
    }

    [MenuItem("Tools/Save room to dungeon _F7")]
    private static void AddRoomsToDungeon() {
        Dungeon dungeon = GameObject.FindObjectOfType<Dungeon>();
        if (dungeon == null) {
            Debug.LogError("No Dungeon generator found");
            return;
        }

        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            Room newRoom = Selection.gameObjects[i].GetComponent<Room>();
            if (newRoom.exits.Count < 1) {
                Debug.LogError("The room:" + newRoom.name + " doesn't have any exit!");
                continue;
            }
            else if(newRoom.exits.Count > (newRoom.size.x*2 + newRoom.size.y*2)) {
                Debug.LogError("The room:" + newRoom.name + " has too many exits!");
                continue;
            }

            RoomToDungeon(newRoom, dungeon);
        }
    }

    private static bool CompareRoomsExits(Room room, Room model) {
        for (int j = 0; j < room.exits.Count; j++) {
            if (room.exits[j].dir != model.exits[j].dir || room.exits[j].pos != model.exits[j].pos) { //implies that exits are stored in the same order
                return false;
            }
        }

        return true;
    }

    private static void RoomToDungeon(Room newRoom, Dungeon dungeon) {
        newRoom.debug = false;

        Undo.RecordObject(dungeon, "Add room to dungeon");

        while (dungeon.zoneRooms.Count <= newRoom.zoneIndex) {
            dungeon.zoneRooms.Add(new ZoneRooms());
            dungeon.zoneRooms[dungeon.zoneRooms.Count - 1].zoneIndex = dungeon.zoneRooms.Count - 1;
        }

        List<RoomList> roomConfigs = dungeon.zoneRooms[newRoom.zoneIndex].roomConfigs;
        for (int i = 0; i < roomConfigs.Count; i++) {
            if (roomConfigs[i].rooms.Contains(newRoom)) {
                Debug.LogWarning("Room NOT added. Zone: " + newRoom.zoneIndex + " Room config: " + i);
                return;
            }

            if (newRoom.exits.Count == roomConfigs[i].rooms[0].exits.Count && CompareRoomsExits(newRoom, roomConfigs[i].rooms[0])) {
                roomConfigs[i].rooms.Add(newRoom);
                Debug.Log("Room added. Zone: " + newRoom.zoneIndex + " Room config: " + i);
                return;
            }
        }

        roomConfigs.Add(new RoomList());
        roomConfigs[roomConfigs.Count - 1].rooms.Add(newRoom);
        Debug.Log("Room added. Zone: " + newRoom.zoneIndex + " Room config: " + (roomConfigs.Count - 1));
    }
}
