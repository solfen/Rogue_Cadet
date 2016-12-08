using UnityEngine;
using UnityEditor;
using System.Collections;

[CreateAssetMenu()]
public class SaveDebugScriptable : ScriptableObject {
    public SaveData data;
}

public class SaveDebugTool {

    [MenuItem("Tools/Load Debug Save _F1")]
    private static void LoadDebugSave() {
        SaveDebugScriptable debugSave = (SaveDebugScriptable)Selection.activeObject;
        debugSave.data = FileSaveLoad.Load();
        Debug.Log(" ----- LOADED ------ ");
    }

    [MenuItem("Tools/Override Save With Debug _F2")]
    private static void OverideSave() {
        SaveDebugScriptable debugSave = (SaveDebugScriptable)Selection.activeObject;
        FileSaveLoad.Save(debugSave.data);
        Debug.Log("----- SAVED ------- ");
    }

    [MenuItem("Tools/Load Debug Save _F1", true)]
    private static bool ValidateLoadDebugSave() {
        return Selection.activeObject.GetType() == typeof(SaveDebugScriptable);
    }

    [MenuItem("Tools/Override Save With Debug _F2", true)]
    private static bool ValidateOverideSave() {
        return Selection.activeObject.GetType() == typeof(SaveDebugScriptable);
    }
}
