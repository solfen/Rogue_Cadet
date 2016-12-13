using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, ISelectHandler {

    [SerializeField] private Image image;
    [SerializeField] private Text nameText;

    private ShopDetailsUI shopDetailsPane;
    private UpgradesShop shop;
    private BaseUpgrade associatedUpgrade;

    public void UpdateItem(BaseUpgrade data, ShopDetailsUI _shopDetailsPane, UpgradesShop upgradeShop) {
        image.sprite = data.sprite;
        nameText.text = data.title;

        shopDetailsPane = _shopDetailsPane;
        shop = upgradeShop;

        GetComponent<Button>().onClick.AddListener(OnClicked);
        associatedUpgrade = GetComponentInChildren<BaseUpgrade>();
    }

    public void OnSelect (BaseEventData data) {
        associatedUpgrade.UpdateDynamicDetails();
        shopDetailsPane.UpdateDetails(associatedUpgrade);
    }

    public void OnClicked() {
        shop.BuyItem(associatedUpgrade);
        OnSelect(null);
    }
}
