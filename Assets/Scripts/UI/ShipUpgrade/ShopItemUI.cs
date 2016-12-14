using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, ISelectHandler {

    [SerializeField] private Image image;
    [SerializeField] private Text nameText;

    private UpgradesShop shop;
    private BaseUpgrade associatedUpgrade;

    public void UpdateItem(BaseUpgrade data, UpgradesShop upgradeShop) {
        image.sprite = data.sprite;
        nameText.text = data.title;
        shop = upgradeShop;

        GetComponent<Button>().onClick.AddListener(OnClicked);
        associatedUpgrade = GetComponentInChildren<BaseUpgrade>();
        associatedUpgrade.UpdateDynamicDetails();
    }

    public void OnSelect (BaseEventData data) {
        shop.OnUpgradeSelected(associatedUpgrade);
    }

    public void OnClicked() {
        shop.BuyItem(associatedUpgrade);
    }
}
