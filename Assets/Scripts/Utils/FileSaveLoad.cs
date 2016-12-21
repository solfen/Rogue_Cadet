using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class FileSaveLoad {

    public static void Save(SaveData data) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");
        bf.Serialize(file, data);
        file.Close();

        EventDispatcher.DispatchEvent(Events.FILE_SAVED, null);
    }

    public static SaveData Load() {
        if (DoesSaveExists()) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else {
            return new SaveData();
        }
    }

    public static bool DoesSaveExists() {
        return File.Exists(Application.persistentDataPath + "/gameSave.dat");
    }
}