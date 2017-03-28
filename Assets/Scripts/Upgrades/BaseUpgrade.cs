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
    public bool unlockedAtStart = false;

    public bool canBuy { get; private set; }
    public bool isUnlocked { get; private set; }
    public bool isMaxed { get; private set; }

    [HideInInspector] public float currentPrice;
    [HideInInspector] public int currentEquipedNb;

    public void UpdateDynamicDetails() {
        SaveData data = GlobalData.instance.saveData;
        GameData gameData = GlobalData.instance.gameData;
        UpgradeInfo upgradeInfo = data.upgradesInfo[saveDataIndex];
        float maxWheight = gameData.shipBaseStats.maxWeight * (gameData.ships[data.selectedShip].maxWheightPercent + data.wheightUpgradeNb * gameData.ships[data.selectedShip].wheightUpgradeRaise);

        float priceMultiplier = upgradeInfo.currentUpgradeNb < upgradeInfo.boughtUpgradeNb ? rebuyMultiplier : 1;
        currentPrice = basePrice * Mathf.Pow(priceMultiplierPerUpgrade, upgradeInfo.currentUpgradeNb) * priceMultiplier;
        currentEquipedNb = upgradeInfo.currentUpgradeNb;

        isMaxed = upgradeInfo.currentUpgradeNb >= numberOfUpgrade;
        canBuy = currentPrice <= data.money && upgradeInfo.isUnlocked && !isMaxed && data.shipWeight + wheight <= maxWheight;
        isUnlocked = upgradeInfo.isUnlocked;
    }

    public abstract void Equip(SaveData dataToModify);
    public abstract void UnEquip(SaveData dataToModify);
}
