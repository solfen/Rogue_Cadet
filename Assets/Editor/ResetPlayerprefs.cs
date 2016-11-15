using UnityEngine;
using System.Collections;
using UnityEditor;


public class ResetPlayerprefs {

    [MenuItem("Tools/Reset Player Prefs")]
    private static void Reset() {
        if(EditorUtility.DisplayDialog("Sure?", "are you sure?", "Shut up", "mistake")) {
            PlayerPrefs.DeleteAll();
        }
    }
}