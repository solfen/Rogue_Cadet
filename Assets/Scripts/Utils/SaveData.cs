using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class UpgradeInfo {
    public int currentUpgradeNb;
    public int boughtUpgradeNb;
    public bool isUnlocked = true;
}

[System.Serializable]
public class ShipInfo {
    public float stock;
    public bool isUnlocked = false;
}

[System.Serializable]
public class SaveData {

    //TODO: Use enum
    public int selectedShip;
    public float shipWeight;
    public float money;
    public List<int> selectedWeapons;
    public int maxWeaponNb;

    public int hitboxUpgradeNb;
    public int lifeUpgradeNb;
    public int manaUpgradeNb;
    public int damageUpgradeNb;
    public int goldUpgradeNb;
    public int bombUpgradeNb;
    public int bombDamageUpgradeNb;

    public List<ShipInfo> shipsInfo;
    public List<UpgradeInfo> upgradesInfo;

}

public static class FileSaveLoad {

    public static void Save(SaveData data) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");
        bf.Serialize(file, data);
        file.Close();

        EventDispatcher.DispatchEvent(Events.FILE_SAVED, null);
    }

    public static SaveData Load() {
        if (File.Exists(Application.persistentDataPath + "/gameSave.dat")) {
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
}
