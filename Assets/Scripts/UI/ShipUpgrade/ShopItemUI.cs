using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, ISelectHandler {

    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject UnavailableImage;
    [SerializeField] private Sprite LockedSprite;

    private UpgradesShop shop;
    private Button button;
    public BaseUpgrade associatedUpgrade { get; private set; }

    public void Init(BaseUpgrade data, UpgradesShop upgradeShop) {
        shop = upgradeShop;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
        associatedUpgrade = GetComponentInChildren<BaseUpgrade>();

        UpdateItem();
    }

    public void UpdateItem() {
        associatedUpgrade.UpdateDynamicDetails();
        UnavailableImage.SetActive(!associatedUpgrade.canBuy && associatedUpgrade.isUnlocked);
        image.sprite = associatedUpgrade.isUnlocked ? associatedUpgrade.sprite : LockedSprite;
        nameText.text = associatedUpgrade.isUnlocked ? associatedUpgrade.title : "????";
    }

    public void OnSelect (BaseEventData data) {
        shop.OnUpgradeSelected(associatedUpgrade);
    }

    public void OnClicked() {
        shop.BuyItem(associatedUpgrade);
    }
}
