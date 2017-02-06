using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveRoomPrefab {

    [MenuItem("Tools/Save room to prefab _F5", true)]
    private static bool Validate() {
        return Selection.activeGameObject != null && Selection.activeTransform.root.GetComponent<Room>() != null;
    }

    [MenuItem("Tools/Save room to prefab _F5")]
    private static void AddRoomsToDungeon() {
        Room room = Selection.activeTransform.root.GetComponent<Room>();
        SaveDynamicContent(room);
        SaveEnemiesPacks(room);

        PrefabUtility.ReplacePrefab(room.gameObject, PrefabUtility.GetPrefabParent(room.gameObject), ReplacePrefabOptions.ConnectToPrefab);

        RestoreSceneRoomPreviousState(room);
    }

    private static void SaveDynamicContent(Room room) {
        RoomDynamicContent[] roomContents = room.GetComponentsInChildren<RoomDynamicContent>();

        Undo.RecordObject(room.gameObject, "Save Room to Prefab");

        for (int i = 0; i < roomContents.Length; i++) {
            roomContents[i].ContentToList();
        }
    }

    private static void SaveEnemiesPacks(Room room) {
        room.enemiesContainers.Clear();

        for (int i = 0; i < room.enemiesParent.childCount; i++) {
            Transform enemiesContainer = room.enemiesParent.GetChild(i);
            EnemyPack pack = new EnemyPack();

            pack.name = enemiesContainer.name;
            pack.container = enemiesContainer.gameObject;

            room.enemiesContainers.Add(pack);

            enemiesContainer.gameObject.SetActive(false);
        }
    }

    private static void RestoreSceneRoomPreviousState(Room room) {
        for (int i = 0; i < room.enemiesParent.childCount; i++) {
            room.enemiesParent.GetChild(i).gameObject.SetActive(true);
        }

        RoomDynamicContent[] roomContents = room.GetComponentsInChildren<RoomDynamicContent>();
        for (int i = 0; i < roomContents.Length; i++) {
            roomContents[i].ListToContent();
        }
    }
}