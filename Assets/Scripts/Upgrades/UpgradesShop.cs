using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradesShop : MonoBehaviour {

    [SerializeField] private ShopUI shopUI;
    [SerializeField] private ShopDetailsUI shopDetailsPane;
    [SerializeField] private WheightUI wheightUI;
    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] private InputMapUI inputMap;

    private int currentCategory;
    private List<UpgradeCategory> upgradesCategories;
    private bool isShopOpen = false;
    private GameData gameData;
    private BaseUpgrade selectedUpgrade;

    // Use this for initialization
    void Start () {
        gameData = GlobalData.instance.gameData;
        upgradesCategories = gameData.upgradesCategories;
        shopUI.InitItems(upgradesCategories);
    }

    void Update() {
        if (Input.GetButtonDown("Start")) {
            inputMap.Open();
        }
        else if (isShopOpen) {
            if(Input.GetButtonDown("Cancel")) {
                shopUI.CloseShop();
                shopDetailsPane.Close();
                isShopOpen = false;
            }
            else if(Input.GetButtonDown("UnEquip")) {
                UnEquip(selectedUpgrade);
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
    public void OnUpgradeSelected(BaseUpgrade associatedUpgrade) {
        selectedUpgrade = associatedUpgrade;
        shopDetailsPane.UpdateDetails(associatedUpgrade);
    }

    public void BuyItem(BaseUpgrade upgrade) {
        if (!upgrade.canBuy) {
            EventDispatcher.DispatchEvent(Events.UI_ERROR, null);
            return;
            //TODO: Error feedback
        }

        SaveData data = FileSaveLoad.Load();
        UpgradeInfo upgradeInfo = data.upgradesInfo[upgrade.saveDataIndex];

        data.money -= upgrade.currentPrice;
        data.shipWeight += upgrade.wheight;
        upgradeInfo.currentUpgradeNb++;
        upgradeInfo.boughtUpgradeNb = Mathf.Max(upgradeInfo.currentUpgradeNb, upgradeInfo.boughtUpgradeNb);

        upgrade.Equip(data); // specialized save data modification. different for each Upgrade type.

        FileSaveLoad.Save(data);

        wheightUI.UpdateWheight();
        moneyUI.UpdateMoney();
        shopUI.UpdateAllItems();
        shopDetailsPane.UpdateDetails(upgrade);
        EventDispatcher.DispatchEvent(Events.UI_SUCCESS, null);
    }

    public void UnEquip(BaseUpgrade upgrade) {
        SaveData data = FileSaveLoad.Load();
        UpgradeInfo upgradeInfo = data.upgradesInfo[upgrade.saveDataIndex];

        if(!upgrade.canUnEquip || upgradeInfo.currentUpgradeNb <= 0) {
            EventDispatcher.DispatchEvent(Events.UI_ERROR, null);
            return;
            //TODO: Error feedback
        }

        data.shipWeight -= upgrade.wheight;
        upgradeInfo.currentUpgradeNb--;
        upgrade.UnEquip(data);

        FileSaveLoad.Save(data);

        wheightUI.UpdateWheight();
        shopUI.UpdateAllItems();
        shopDetailsPane.UpdateDetails(upgrade);
        EventDispatcher.DispatchEvent(Events.UI_SUCCESS, null);
    }
}
