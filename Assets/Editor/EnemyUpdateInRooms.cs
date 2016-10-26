using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class EnemyUpdateInRooms {

    private static string[] roomsPaths = { "Assets/Tiled2Unity/Prefabs" };

    [MenuItem("Tools/Update enemy in rooms _F6")]
    private static void UpdateEnemiesRoom() {
        string regex = "\\b" + Selection.activeGameObject.name + "\\b|\\b" + Selection.activeGameObject.name + "(Clone)" + "\\b"; //regex to match for Enemy name whole word or whole word + clone
        List<GameObject> rooms = new List<GameObject>();
        string[] guids = AssetDatabase.FindAssets("t:GameObject", roomsPaths);

        foreach (string guid in guids) {
            rooms.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)));
        }

        if (rooms.Count < 1) {
            Debug.LogError("No rooms at " + roomsPaths[0] + ". Make sure it's the right path");
        }

        int cpt = 0;
        for (int i = 0; i < rooms.Count; i++) {
            Enemy[] enemies = rooms[i].GetComponent<Room>().enemiesParent.GetComponentsInChildren<Enemy>(true);
            for (int j = enemies.Length-1; j >= 0; j--) {
                if (Regex.IsMatch(enemies[j].name, regex)) {
                    Object.Instantiate(Selection.activeGameObject, enemies[j].transform.position, enemies[j].transform.rotation, enemies[j].transform.parent);
                    Object.DestroyImmediate(enemies[j].gameObject, true);
                    cpt++;
                }
            }

            EditorUtility.SetDirty(rooms[i]);
        }


        Debug.Log("Room enemies updated: " + cpt);
    }

    [MenuItem("Tools/Update enemy in rooms _F6", true)]
    private static bool UpdateEnemiesRoomValidate() {
        return Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Enemy>() != null;
    }
}
