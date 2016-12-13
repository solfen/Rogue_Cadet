using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUI : MonoBehaviour {

    [SerializeField] private ShopDetailsUI shopDetailPane;
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
    private float money;
    private bool isOpen;
    private GameObject lastCategorySelected;
    private GameObject activeCategoryContainer = null;
    private Dictionary<UpgradeCategory, GameObject> categoriesContainers = new Dictionary<UpgradeCategory, GameObject>();

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
                GameObject item = Instantiate(shopItemPrefab, containerTransform, false) as GameObject;
                Instantiate(upgradesCategories[i].upgrades[j], item.transform, false);
                item.GetComponent<ShopItemUI>().UpdateItem(upgradesCategories[i].upgrades[j], shopDetailPane, upgradeShop);
            }

            container.SetActive(false);
            categoriesContainers.Add(upgradesCategories[i], container);
        }
    }

    public void OpenShop(UpgradeCategory category) {
        lastCategorySelected = eventSystem.currentSelectedGameObject;

        anim.SetTrigger("OpenShop");

        ActivateCategory(category);
        shopDetailPane.Open(); //needs to be before ActivateCategory because selecting the first object calls an update on the details (berk)

        isOpen = true;
    }

    public void CloseShop() {
        eventSystem.SetSelectedGameObject(lastCategorySelected);
        anim.SetTrigger("CloseShop");
        shopDetailPane.Close();
        isOpen = false;
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
