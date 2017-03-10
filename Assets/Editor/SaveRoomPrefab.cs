using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveRoomPrefab {

    private static RoomDynamicContent[] lastRoomContents;
    [MenuItem("Tools/Save room to prefab _F5", true)]
    private static bool Validate() {
        return Selection.activeGameObject != null && Selection.activeTransform != null && Selection.activeTransform.root.GetComponent<Room>() != null;
    }

    [MenuItem("Tools/Save room to prefab _F5")]
    private static void AddRoomsToDungeon() {
        Room room = Selection.activeTransform.root.GetComponent<Room>();
        room.debug = false;
        SaveDynamicContent(room);
        SaveEnemiesPacks(room);

        PrefabUtility.ReplacePrefab(room.gameObject, PrefabUtility.GetPrefabParent(room.gameObject), ReplacePrefabOptions.ConnectToPrefab);

        RestoreSceneRoomPreviousState(room);
    }

    private static void SaveDynamicContent(Room room) {
        lastRoomContents = room.GetComponentsInChildren<RoomDynamicContent>();

        Undo.RecordObject(room.gameObject, "Save Room to Prefab");

        for (int i = 0; i < lastRoomContents.Length; i++) {
            lastRoomContents[i].ContentToList();
            lastRoomContents[i].enabled = true;
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
        for (int i = 0; i < lastRoomContents.Length; i++) {
            lastRoomContents[i].gameObject.SetActive(true);
        }

        room.debug = true;
    }
}