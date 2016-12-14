using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradesShop : MonoBehaviour {

    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ShopDetailsUI shopDetailsPane;
    [SerializeField] private WheightUI wheightUI;
    [SerializeField] private MoneyUI moneyUI;

    private int currentCategory;
    private List<UpgradeCategory> upgradesCategories;
    private bool isShopOpen = false;
    private GameData gameData;
    private ShopItemUI selectedShopItem;

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
        else if (isShopOpen) {
            if(Input.GetButtonDown("Cancel")) {
                shopUI.CloseShop();
                shopDetailsPane.Close();
                isShopOpen = false;
            }
            else if(Input.GetButtonDown("UnEquip")) {
                UnEquip(selectedShopItem);
            }
        }
    }

    //called from UI
    public void OnCategorySelected(int index) {
        shopUI.OpenShop(upgradesCategories[index]);
        shopDetailsPane.Open();
        isShopOpen = true;
    }

    //called from ShopItemUI.OnSelected event
    public void OnUpgradeSelected(ShopItemUI item) {
        selectedShopItem = item;
        shopDetailsPane.UpdateDetails(item.associatedUpgrade);
    }

    public void BuyItem(ShopItemUI item) {
        BaseUpgrade upgrade = item.associatedUpgrade;
        SaveData data = FileSaveLoad.Load();
        UpgradeInfo upgradeInfo = data.upgradesInfo[upgrade.saveDataIndex];

        if (!upgrade.canBuy) {
            return;
            //TODO: Error feedback
        }

        data.money -= upgrade.currentPrice;
        data.shipWeight += upgrade.wheight;
        upgradeInfo.currentUpgradeNb++;
        upgradeInfo.boughtUpgradeNb = Mathf.Max(upgradeInfo.currentUpgradeNb, upgradeInfo.boughtUpgradeNb);

        upgrade.Equip(data); // specialized save data modification. different for each Upgrade type.

        FileSaveLoad.Save(data);

        wheightUI.UpdateWheight();
        moneyUI.UpdateMoney();
        item.UpdateItem();
        shopDetailsPane.UpdateDetails(upgrade);
    }

    public void UnEquip(ShopItemUI item) {
        BaseUpgrade upgrade = item.associatedUpgrade;
        SaveData data = FileSaveLoad.Load();
        UpgradeInfo upgradeInfo = data.upgradesInfo[upgrade.saveDataIndex];

        if(!upgrade.canUnEquip || upgradeInfo.currentUpgradeNb <= 0) {
            return;
            //TODO: Error feedback
        }

        data.shipWeight -= upgrade.wheight;
        upgradeInfo.currentUpgradeNb--;
        upgrade.UnEquip(data);

        FileSaveLoad.Save(data);

        wheightUI.UpdateWheight();
        item.UpdateItem();
        shopDetailsPane.UpdateDetails(upgrade);
    }
}
