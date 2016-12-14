using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateNewSave : MonoBehaviour {

    void Start() {
        Create(); //tmp for debug
    }

    public void Create() {
        SaveData data = FileSaveLoad.Load();
        List<ShipConfig> ships = GlobalData.instance.gameData.ships;

        //TODO upgrades init

        data.selectedWeapons = new List<int>();
        data.selectedWeapons.Add(0);

        data.shipsInfo = new List<ShipInfo>();
        for(int i = 0; i < ships.Count; i++) {
            ShipInfo info = new ShipInfo();
            info.isUnlocked = i < 3;
            info.stock = ships[i].maxStock;
            data.shipsInfo.Add(info);
        }

        FileSaveLoad.Save(data);
    }
}
