using UnityEngine;
using UnityEditor;

public class EnemyPackCreationTool {
    [MenuItem("Tools/Save Enemy Pack _F5")]
    private static void SaveEnemiesPacks() {
        Room room = Selection.activeTransform.root.GetComponent<Room>();
        room.enemiesContainers.Clear();

        for (int i = 0; i < room.enemiesParent.childCount; i++) {
            Transform enemiesContainer = room.enemiesParent.GetChild(i);
            EnemyPack pack = new EnemyPack();

            pack.probabilityMultiplier = 1;
            pack.name = enemiesContainer.name;
            pack.container = enemiesContainer.gameObject;

            room.enemiesContainers.Add(pack);
        }

        PrefabUtility.ReplacePrefab(room.gameObject, PrefabUtility.GetPrefabParent(room.gameObject), ReplacePrefabOptions.ConnectToPrefab);
    }

    [MenuItem("Tools/Save Enemy Pack _F5", true)]
    private static bool SavePackMenuItemValidation() {
        return Selection.activeTransform != null
            && Selection.activeTransform.root.GetComponent<Room>() != null;
    }
}
