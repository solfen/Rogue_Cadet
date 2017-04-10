using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, ISelectHandler {

    [SerializeField] private Image image;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject UnavailableImage;
    [SerializeField] private GameObject MaxedImage;
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
        MaxedImage.SetActive(associatedUpgrade.isMaxed);
        UnavailableImage.SetActive(!associatedUpgrade.canBuy && associatedUpgrade.isUnlocked && !associatedUpgrade.isMaxed);
        image.sprite = associatedUpgrade.isUnlocked ? associatedUpgrade.sprite : LockedSprite;
        nameText.text = associatedUpgrade.isUnlocked ? LocalizationManager.GetLocalizedText(associatedUpgrade.title) : "????";
    }

    public void OnSelect (BaseEventData data) {
        shop.OnUpgradeSelected(associatedUpgrade);
        EventDispatcher.DispatchEvent(Events.SELECT_UI, null);
    }

    public void OnClicked() {
        shop.BuyItem(associatedUpgrade);
    }
}
