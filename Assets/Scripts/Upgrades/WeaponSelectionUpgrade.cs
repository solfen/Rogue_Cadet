using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSelectionUpgrade : BaseUpgrade {

    public int weaponIndex;
    [SerializeField] private int weaponUpgradesCategory = 3;
    
    public override void Equip(SaveData dataToModify) {
        if(dataToModify.selectedWeapons.Count >= dataToModify.maxWeaponNb) {
            DeactiveWeaponUpgrade(dataToModify);
        }

        dataToModify.selectedWeapons.Add(weaponIndex);
    }

    public override void UnEquip(SaveData dataToModify) {
        dataToModify.selectedWeapons.Remove(weaponIndex);
    }

    private void DeactiveWeaponUpgrade(SaveData data) {
        List<BaseUpgrade> weaponUpgrades = GlobalData.instance.gameData.upgradesCategories[weaponUpgradesCategory].upgrades;
        for (int i = 0; i < weaponUpgrades.Count; i++) {
            WeaponSelectionUpgrade weaponUpgrade = (WeaponSelectionUpgrade)weaponUpgrades[i];
            if (weaponUpgrade == null) {
                Debug.LogError("Upgrade in weapon upgrade cateogry is NOT a weapon upgrade!");
            }

            if (weaponUpgrade.weaponIndex == data.selectedWeapons[0]) { //manually unequip first selected weapon
                data.shipWeight -= weaponUpgrade.wheight;
                data.upgradesInfo[weaponUpgrade.saveDataIndex].currentUpgradeNb--;
                weaponUpgrade.UnEquip(data);
                break;
            }
        }
    }
}
