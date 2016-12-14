using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockShipsUpgrade : BaseUpgrade {

    [SerializeField] private List<int> shipsIndexesToUnlock;

    public override void Equip(SaveData dataToModify) {
        for (int i = 0; i < shipsIndexesToUnlock.Count; i++) {
            dataToModify.shipsInfo[shipsIndexesToUnlock[i]].isUnlocked = true;
        }
    }

    public override void UnEquip(SaveData dataToModify) {
        Debug.LogError("Unlock upgrades are not supposed to be un-equipable. This induces dependencies problems");
    }
}
