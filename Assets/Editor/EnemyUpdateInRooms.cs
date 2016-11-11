﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class EnemyUpdateInRooms {

    private static string[] roomsPaths = { "Assets/Tiled2Unity/Prefabs", "Assets/Tiled2Unity/Prefabs/Zone1", "Assets/Tiled2Unity/Prefabs/Zone2", "Assets/Tiled2Unity/Prefabs/Zone3" };

    [MenuItem("Tools/Update enemy in rooms _F6")]
    private static void UpdateEnemiesRoom() {

        for (int i = 0; i < Selection.gameObjects.Length; i++) {
            string regex = "\\b" + Selection.gameObjects[i].name + "\\b|\\b" + Selection.gameObjects[i].name + "(Clone)" + "\\b"; //regex to match for Enemy name whole word or whole word + clone
            List<GameObject> rooms = new List<GameObject>();
            string[] guids = AssetDatabase.FindAssets("t:GameObject", roomsPaths);

            foreach (string guid in guids) {
                rooms.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)));
            }

            if (rooms.Count < 1) {
                Debug.LogError("No rooms at " + roomsPaths[0] + ". Make sure it's the right path");
                return;
            }

            int cpt = 0;
            for (int j = 0; j < rooms.Count; j++) {
                Enemy[] enemies = rooms[j].GetComponent<Room>().enemiesParent.GetComponentsInChildren<Enemy>(true);
                for (int k = enemies.Length - 1; k >= 0; k--) {
                    if (Regex.IsMatch(enemies[k].name, regex)) {
                        Object.Instantiate(Selection.gameObjects[i], enemies[k].transform.position, enemies[k].transform.rotation, enemies[k].transform.parent);
                        Object.DestroyImmediate(enemies[k].gameObject, true);
                        cpt++;
                    }
                }

                EditorUtility.SetDirty(rooms[j]);
            }


            Debug.Log("nb enemies " + Selection.gameObjects[i].name + " updated: " + cpt);
        }
    }

    [MenuItem("Tools/Update enemy in rooms _F6", true)]
    private static bool UpdateEnemiesRoomValidate() {
        return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Enemy>() != null;
    }
}
