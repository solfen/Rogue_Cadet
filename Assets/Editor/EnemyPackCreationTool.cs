using UnityEngine;
using UnityEditor;

public class EnemyPackCreationTool {
   // [MenuItem("Tools/Save Enemy Pack")]
    private static void SaveEnemiesPacks() {
        Room room = Selection.activeTransform.root.GetComponent<Room>();
        room.enemiesContainers.Clear();

        for (int i = 0; i < room.enemiesParent.childCount; i++) {
            Transform enemiesContainer = room.enemiesParent.GetChild(i);
            EnemyPack pack = new EnemyPack();

            pack.name = enemiesContainer.name;
            pack.container = enemiesContainer.gameObject;

            room.enemiesContainers.Add(pack);

            enemiesContainer.gameObject.SetActive(false);
        }

        PrefabUtility.ReplacePrefab(room.gameObject, PrefabUtility.GetPrefabParent(room.gameObject), ReplacePrefabOptions.ConnectToPrefab);
    }

   // [MenuItem("Tools/Save Enemy Pack", true)]
    private static bool SavePackMenuItemValidation() {
        return Selection.activeTransform != null
            && Selection.activeTransform.root.GetComponent<Room>() != null;
    }
}
