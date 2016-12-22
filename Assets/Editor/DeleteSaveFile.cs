using UnityEngine;
using UnityEditor;
using System.IO;

public class DeleteSaveFile {

    [MenuItem("Tools/Delete Save File")]
    private static void DeleteFile() {
        File.Delete(Application.persistentDataPath + "/gameSave.dat");
        Debug.Log("----- DELETED ------");
    }

    [MenuItem("Tools/Delete Save File", true)]
    private static bool DeleteFileCheck() {
        return FileSaveLoad.DoesSaveExists();
    }
}
