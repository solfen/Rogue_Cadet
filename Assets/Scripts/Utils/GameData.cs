﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameData : ScriptableObject {
    public Vector2 worldSize;
    public Vector2 roomBaseSize;
    public List<Zone> zones;
    public List<Weapon> weapons;
    public List<GameObject> bombs;
    public ShipBaseConfig shipBaseStats;
    public List<ShipConfig> ships;
    public List<ShipsUIItemData> shipsUIItems;
}

[System.Serializable]
public class ShipBaseConfig {
    public float maxLife = 100f;
    public float maxMana = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public Vector2 hitboxSize;
}

[System.Serializable]
public class ShipConfig {
    public string name;
    public float lifePrecent;
    public float fireRatePrecent;
    public float damagePrecent;
    public float manaPrecent;
    public float goldPercent;
    public float hitboxSizePercent;
    public float speed;
    public float meleeDamagePercent;
    public float invicibiltyDurationPercent = 1f;
}

[System.Serializable]
public class ShipsUIItemData {
    public string name;
    public Sprite spriteUI;
    public List<ShipTypeUIItem> types;
}

[System.Serializable]
public class ShipTypeUIItem {
    public string description;
    public string powerName;
    public string powerDescription;
    public Sprite typeSprite;
    public int associatedShipIndex;
}
