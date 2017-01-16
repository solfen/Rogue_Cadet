using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameData : ScriptableObject {
    public Vector2 worldSize;
    public Vector2 roomBaseSize;
    public List<Zone> zones;
    public List<BaseWeapon> weapons;
    public ShipBaseConfig shipBaseStats;
    public List<int> shipsUnlockedAtStart;
    public List<ShipConfig> ships;
    public List<ShipsUIItemData> shipsUIItems;
    public List<UpgradeCategory> upgradesCategories;
    public List<AchievementUI> achievementsUI;
}

[System.Serializable]
public class ShipBaseConfig {
    public float maxLife = 100f;
    public float maxMana = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public Vector2 hitboxSize;
    public float maxWeight = 100;
}

[System.Serializable]
public class ShipConfig {
    public string name;
    public Player playerPrefab;

    [Header("Stats")]
    public float lifePrecent;
    public float fireRatePrecent;
    public float damagePrecent;
    public float manaPrecent;
    public float goldPercent;
    public float hitboxSizePercent;
    public float speed;
    public float meleeDamagePercent;
    public float maxWheightPercent;
    public float invicibiltyDurationPercent = 1f;

    [Header("Upgrade Influence")]
    public float lifeUpgradeRaise;
    public float hitboxUpgradeRaise;
    public float manaUpgradeRaise;
    public float damageUpgradeRaise;
    public float wheightUpgradeRaise;
    public float goldUpgradeRaise;
    public int bombStockUpgradeRaise;
    public float bombDamagePerUpgrade;

    [Header("Stock")]
    public int maxStock = 5;
    public float stockGainByRun = 0.5f;
}

[System.Serializable]
public class ShipsUIItemData {
    public string name;
    public Sprite spriteUI;
    public int associatedShipIndex;
    public List<ShipTypeUIItem> types;
}

[System.Serializable]
public class ShipTypeUIItem {
    public string name;
    public string description;
    public string powerName;
    public string powerDescription;
    public Sprite typeSprite;
    public int associatedShipIndex;
    public int associatedAchievementIndex = -1;
}

[System.Serializable]
public class UpgradeCategory {
    public List<BaseUpgrade> upgrades;
}

[System.Serializable]
public class AchievementUI {
    public string name;
    public string description;
    public Sprite icon; 
}
