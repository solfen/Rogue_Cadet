using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradesShop : MonoBehaviour {

    [SerializeField] private ShopUI shopUI;
    [SerializeField] private WheightUI wheightUI;
    [SerializeField] private MoneyUI moneyUI;

    private int currentCategory;
    private List<UpgradeCategory> upgradesCategories;
    private bool isShopOpen = false;
    private GameData gameData;

    // Use this for initialization
    void Start () {
        gameData = GlobalData.instance.gameData;
        upgradesCategories = gameData.upgradesCategories;
        shopUI.InitItems(upgradesCategories);
    }

    void Update() {
        if (Input.GetButtonDown("Start")) {
            InputMapUI.instance.Open();
        }
        else if (isShopOpen && Input.GetButtonDown("Cancel")) {
            shopUI.CloseShop();
            isShopOpen = false;
        }
    }

    //called from UI
    public void OnCategorySelected(int index) {
        shopUI.OpenShop(upgradesCategories[index]);
        isShopOpen = true;
    }

    public void BuyItem(BaseUpgrade upgrade) {
        SaveData data = FileSaveLoad.Load();
        UpgradeInfo upgradeInfo = data.upgradesInfo[upgrade.saveDataIndex];

        float maxWheight = gameData.ships[data.selectedShip].maxWheightPercent * gameData.shipBaseStats.maxWeight;

        if (upgrade.currentPrice > data.money || !upgradeInfo.isUnlocked || upgradeInfo.currentUpgradeNb >= upgrade.numberOfUpgrade || data.shipWeight + upgrade.wheight > maxWheight) {
            return;
        }

        data.money -= upgrade.currentPrice;
        data.shipWeight += upgrade.wheight;
        upgradeInfo.currentUpgradeNb++;
        upgradeInfo.boughtUpgradeNb = Mathf.Max(upgradeInfo.currentUpgradeNb, upgradeInfo.boughtUpgradeNb);

        upgrade.Equip(data); // specialized save data modification. different for each Upgrade type.

        FileSaveLoad.Save(data);

        wheightUI.UpdateWheight();
        moneyUI.UpdateMoney();
    }
}
