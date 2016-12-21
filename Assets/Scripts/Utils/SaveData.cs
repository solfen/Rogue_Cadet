﻿using System.Collections.Generic;


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