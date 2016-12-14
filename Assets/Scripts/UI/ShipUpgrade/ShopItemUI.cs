using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, ISelectHandler {

    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject UnavailableImage;

    private UpgradesShop shop;
    private Button button;
    public BaseUpgrade associatedUpgrade { get; private set; }

    public void Init(BaseUpgrade data, UpgradesShop upgradeShop) {
        image.sprite = data.sprite;
        nameText.text = data.title;
        shop = upgradeShop;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
        associatedUpgrade = GetComponentInChildren<BaseUpgrade>();

        UpdateItem();
    }

    public void UpdateItem() {
        associatedUpgrade.UpdateDynamicDetails();
        UnavailableImage.SetActive(!associatedUpgrade.canBuy);
    }

    public void OnSelect (BaseEventData data) {
        shop.OnUpgradeSelected(this);
    }

    public void OnClicked() {
        shop.BuyItem(this);
    }
}
