using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameData : ScriptableObject {
    public Vector2 worldSize;
    public Vector2 roomBaseSize;
    public List<Zone> zones;
    public List<Weapon> weapons;
    public List<GameObject> bombs;
    public List<ShipsData> ships;
}

[System.Serializable]
public class ShipsData {
    public string name;
    public Sprite spriteUI;
    public List<ShipType> types;
}

[System.Serializable]
public class ShipType {
    public string description;
    public string powerName;
    public string powerDescription;
    public Sprite typeSprite;
    public Player associatedShip;
    public SpecialPower associatedPower;
}
