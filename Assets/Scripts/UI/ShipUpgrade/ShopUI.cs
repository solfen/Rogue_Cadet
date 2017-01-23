using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUI : MonoBehaviour {

    [SerializeField] private UpgradesShop upgradeShop;
    [SerializeField] private GameObject categroryContainerPrefab;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Text moneyText;

    private Transform _transform;
    private ResponsiveGridLayout gridLayout;
    private int currentCategory;
    private int currentItem;
    private GameObject lastCategorySelected;
    private GameObject activeCategoryContainer = null;
    private Dictionary<UpgradeCategory, GameObject> categoriesContainers = new Dictionary<UpgradeCategory, GameObject>();
    private List<ShopItemUI> items = new List<ShopItemUI>();

    void Awake() {
        _transform = GetComponent<Transform>();
        gridLayout = GetComponent<ResponsiveGridLayout>();
    }

    public void InitItems(List<UpgradeCategory> upgradesCategories) {

        for (int i = 0; i < upgradesCategories.Count; i++) {
            GameObject container = Instantiate(categroryContainerPrefab, _transform, false) as GameObject;
            Transform containerTransform = container.GetComponent<Transform>();
            RectTransform containerRectTrans = container.GetComponent<RectTransform>();
            containerRectTrans.anchorMin = Vector2.zero;
            containerRectTrans.anchorMax = new Vector2(1, 1);

            for (int j = 0; j < upgradesCategories[i].upgrades.Count; j++) {
                BaseUpgrade upgradePrefab = upgradesCategories[i].upgrades[j];
                GameObject itemGO = Instantiate(shopItemPrefab, containerTransform, false) as GameObject;
                ShopItemUI item = itemGO.GetComponent<ShopItemUI>();
                Instantiate(upgradePrefab, itemGO.transform, false);
                item.Init(upgradePrefab, upgradeShop);

                items.Add(item);
            }

            container.SetActive(false);
            categoriesContainers.Add(upgradesCategories[i], container);
        }
    }

    public void UpdateAllItems() {
        for(int i = 0; i < items.Count; i++) {
            items[i].UpdateItem();
        }
    }

    public void OpenShop(UpgradeCategory category) {
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        lastCategorySelected = eventSystem.currentSelectedGameObject;
        anim.SetTrigger("OpenShop");
        ActivateCategory(category);
    }

    public void CloseShop() {
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
        eventSystem.SetSelectedGameObject(lastCategorySelected);
        anim.SetTrigger("CloseShop");
    }

    private void ActivateCategory(UpgradeCategory category) {
        if(activeCategoryContainer != null) {
            activeCategoryContainer.SetActive(false);
        }

        activeCategoryContainer = categoriesContainers[category];
        activeCategoryContainer.SetActive(true);

        gridLayout.target = activeCategoryContainer.transform;
        gridLayout.Resize();

        eventSystem.SetSelectedGameObject(activeCategoryContainer.transform.GetChild(0).gameObject);
    }
}
