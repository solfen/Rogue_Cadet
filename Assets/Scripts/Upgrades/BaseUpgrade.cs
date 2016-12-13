using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class BaseUpgrade : MonoBehaviour {

    public int saveDataIndex;
    public string title;
    public string description;
    public Sprite sprite;
    public float basePrice;
    public float priceMultiplierPerUpgrade;
    public float rebuyMultiplier;
    public int numberOfUpgrade;
    public float wheight;
    public bool canUnEquip = true;

    [HideInInspector] public float currentPrice;
    [HideInInspector] public int currentEquipedNb;

    public void UpdateDynamicDetails() {
        SaveData data = FileSaveLoad.Load();
        UpgradeInfo upgradeInfo = data.upgradesInfo[saveDataIndex];

        float priceMultiplier = upgradeInfo.currentUpgradeNb < upgradeInfo.boughtUpgradeNb ? rebuyMultiplier : 1;
        currentPrice = basePrice * Mathf.Pow(upgradeInfo.currentUpgradeNb + 1, priceMultiplierPerUpgrade) * priceMultiplier;
        currentEquipedNb = upgradeInfo.currentUpgradeNb;
    }

    public abstract void Equip(SaveData dataToModify);
    public abstract void UnEquip(SaveData dataToModify);
}
