using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradesUnlockUpgrade : BaseUpgrade {

    [SerializeField] private List<int> upgradesIndexesToUnlock;

    public override void Equip(SaveData dataToModify) {
        for (int i = 0; i < upgradesIndexesToUnlock.Count; i++) {
            dataToModify.upgradesInfo[upgradesIndexesToUnlock[i]].isUnlocked = true;
        }
    }

    public override void UnEquip(SaveData dataToModify) {
        Debug.LogError("Unlock upgrades are not supposed to be un-equipable. This induces dependencies problems");
    }
}
