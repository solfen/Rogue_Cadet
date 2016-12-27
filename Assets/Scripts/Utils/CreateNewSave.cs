using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateNewSave : MonoBehaviour {

    void Start() {
        if(!FileSaveLoad.DoesSaveExists()) {
            SaveData data = FileSaveLoad.Load();
            List<ShipConfig> ships = GlobalData.instance.gameData.ships;
            List<UpgradeCategory> upgradesCategories = GlobalData.instance.gameData.upgradesCategories;

            //TODO upgrades init

            data.selectedWeapons = new List<int>();
            data.selectedWeapons.Add(0);

            data.shipsInfo = new List<ShipInfo>();
            for (int i = 0; i < ships.Count; i++) {
                ShipInfo info = new ShipInfo();
                info.isUnlocked = i < 3;
                info.stock = ships[i].maxStock;
                data.shipsInfo.Add(info);
            }

            data.upgradesInfo = new List<UpgradeInfo>();
            for(int i = 0; i < upgradesCategories.Count; i++) {
                for(int j = 0; j < upgradesCategories[i].upgrades.Count; j++) {
                    UpgradeInfo info = new UpgradeInfo();
                    info.isUnlocked = upgradesCategories[i].upgrades[j].unlockedAtStart;
                    data.upgradesInfo.Add(info);
                }
            }

            FileSaveLoad.Save(data);
        }
    }
}
