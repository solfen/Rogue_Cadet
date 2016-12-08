using UnityEngine;
using UnityEditor;
using System.Collections;

public class SaveDebugTool {

    [MenuItem("Tools/Load Debug Save _F9")]
    private static void LoadDebugSave() {
        SaveDebugScriptable debugSave = (SaveDebugScriptable)Selection.activeObject;
        debugSave.data = FileSaveLoad.Load();
        Debug.Log(" ----- LOADED ------ ");
    }

    [MenuItem("Tools/Override Save With Debug _F10")]
    private static void OverideSave() {
        SaveDebugScriptable debugSave = (SaveDebugScriptable)Selection.activeObject;
        FileSaveLoad.Save(debugSave.data);
        Debug.Log("----- SAVED ------- ");
    }

    [MenuItem("Tools/Load Debug Save _F9", true)]
    private static bool ValidateLoadDebugSave() {
        return Selection.activeObject.GetType() == typeof(SaveDebugScriptable);
    }

    [MenuItem("Tools/Override Save With Debug _F10", true)]
    private static bool ValidateOverideSave() {
        return Selection.activeObject.GetType() == typeof(SaveDebugScriptable);
    }
}
