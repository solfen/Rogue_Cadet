using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateNewSave : MonoBehaviour {

    [SerializeField] private WeaponSelectionUpgrade firstWeapon;

    void Start() {
        if(!FileSaveLoad.DoesSaveExists()) {
            SaveData data = FileSaveLoad.Load();
            List<ShipConfig> ships = GlobalData.instance.gameData.ships;
            List<UpgradeCategory> upgradesCategories = GlobalData.instance.gameData.upgradesCategories;

            data.shipsInfo = new List<ShipInfo>();
            for (int i = 0; i < ships.Count; i++) {
                ShipInfo info = new ShipInfo();
                info.stock = ships[i].maxStock;
                data.shipsInfo.Add(info);
            }

            for(int i = 0; i < GlobalData.instance.gameData.shipsUnlockedAtStart.Count; i++) {
                data.shipsInfo[GlobalData.instance.gameData.shipsUnlockedAtStart[i]].isUnlocked = true;
            }

            data.upgradesInfo = new List<UpgradeInfo>();
            for(int i = 0; i < upgradesCategories.Count; i++) {
                for(int j = 0; j < upgradesCategories[i].upgrades.Count; j++) {
                    UpgradeInfo info = new UpgradeInfo();
                    info.isUnlocked = upgradesCategories[i].upgrades[j].unlockedAtStart;
                    data.upgradesInfo.Add(info);
                }
            }

            data.selectedWeapons = new List<int>();
            data.selectedWeapons.Add(firstWeapon.weaponIndex);
            data.shipWeight += firstWeapon.wheight;
            data.upgradesInfo[firstWeapon.saveDataIndex].boughtUpgradeNb = 1;
            data.upgradesInfo[firstWeapon.saveDataIndex].currentUpgradeNb = 1;

            FileSaveLoad.Save(data);
        }
    }
}
