using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseUpgrade : MonoBehaviour {

    public string title;
    public string description;
    public Sprite sprite;
    public float price;
    public int numberOfUpgrade;
    public float wheight;
    public bool canUnEquip = true;

    public abstract void Equip();
    public abstract void UnEquip();
}
