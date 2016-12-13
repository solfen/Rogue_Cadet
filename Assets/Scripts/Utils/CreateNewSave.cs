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

        data.shipsStock = new List<float>();
        for(int i = 0; i < ships.Count; i++) {
            data.shipsStock.Add(ships[i].maxStock);
        }

        FileSaveLoad.Save(data);
    }
}
