using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class EnemyUpdateInRooms {

    private static string[] roomsPaths = { "Assets/Prefabs/Rooms/Test" }; //{ "Assets/Prefabs/Rooms", "Assets/Prefabs/Rooms/Zone1", "Assets/Prefabs/Rooms/Zone2", "Assets/Prefabs/Rooms/Zone3" };

    [MenuItem("Tools/Update enemy in rooms _F6")]
    private static void UpdateEnemiesRoom() {
        List<GameObject> rooms = new List<GameObject>();
        string[] guids = AssetDatabase.FindAssets("t:GameObject", roomsPaths);

        foreach (string guid in guids) {
            rooms.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        if (rooms.Count < 1) {
            Debug.LogError("No rooms at " + roomsPaths[0] + ". Make sure it's the right path");
            return;
        }

        int totalEnemies = 0;
        for (int i = 0; i < rooms.Count; i++) {
            GameObject roomGameObject = Object.Instantiate(rooms[i]);
            roomGameObject.name = rooms[i].name;
            Enemy[] enemies = roomGameObject.GetComponent<Room>().enemiesParent.GetComponentsInChildren<Enemy>(true);
            int enemiesRoomUpdated = 0;

            for (int j = 0; j < Selection.gameObjects.Length; j++) {
                string regex = "\\b" + Selection.gameObjects[j].name + "\\b|\\b" + Selection.gameObjects[j].name + "(Clone)" + "\\b"; //regex to match for Enemy name whole word or whole word + clone
                for (int k = enemies.Length - 1; k >= 0; k--) {
                    if (enemies[k] != null && Regex.IsMatch(enemies[k].name, regex)) {
                        Object.Instantiate(Selection.gameObjects[j], enemies[k].transform.position, enemies[k].transform.rotation, enemies[k].transform.parent);
                        Object.DestroyImmediate(enemies[k].gameObject, true);
                        totalEnemies++;
                        enemiesRoomUpdated++;
                    }
                }
            }

            if(enemiesRoomUpdated > 0)
                PrefabUtility.ReplacePrefab(roomGameObject, rooms[i], ReplacePrefabOptions.ReplaceNameBased);
            Object.DestroyImmediate(roomGameObject);
        }

        Debug.Log("nb enemies updated: " + totalEnemies);
    }

    [MenuItem("Tools/Update enemy in rooms _F6", true)]
    private static bool UpdateEnemiesRoomValidate() {
        return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Enemy>() != null;
    }
}
