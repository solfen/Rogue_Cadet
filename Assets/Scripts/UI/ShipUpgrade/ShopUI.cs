using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUI : MonoBehaviour {

    [SerializeField] private ShopDetailsUI shopDetailPane;
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
    private Dictionary<ShopCategory, GameObject> categoriesContainers = new Dictionary<ShopCategory, GameObject>();

    void Awake() {
        _transform = GetComponent<Transform>();
        gridLayout = GetComponent<ResponsiveGridLayout>();
    }

    void Update () {
        if (isOpen && Input.GetButtonDown("Cancel")) {
            CloseShop();
        }
    }

    public void InitItems(List<ShopCategory> itemsCategories) {
        Debug.Log(itemsCategories.Count);
        for (int i = 0; i < itemsCategories.Count; i++) {
            GameObject container = Instantiate(categroryContainerPrefab, _transform, false) as GameObject;
            Transform containerTransform = container.GetComponent<Transform>();
            RectTransform containerRectTrans = container.GetComponent<RectTransform>();
            containerRectTrans.anchorMin = Vector2.zero;
            containerRectTrans.anchorMax = new Vector2(1, 1);

            for (int j = 0; j < itemsCategories[i].items.Count; j++) {
                GameObject item = Instantiate(shopItemPrefab, containerTransform, false) as GameObject;
                item.GetComponent<ShopItemUI>().UpdateItem(itemsCategories[i].items[j], j, shopDetailPane);
            }

            container.SetActive(false);
            categoriesContainers.Add(itemsCategories[i], container);
        }
    }

    public void OpenShop(ShopCategory category) {
        lastCategorySelected = eventSystem.currentSelectedGameObject;

        shopDetailPane.Open(category); //needs to be before ActivateCategory because selecting the first object calls an update on the details (berk)
        anim.SetTrigger("OpenShop");

        ActivateCategory(category);

        isOpen = true;
    }

    public void CloseShop() {
        eventSystem.SetSelectedGameObject(lastCategorySelected);
        anim.SetTrigger("CloseShop");
        shopDetailPane.Close();
        isOpen = false;
    }

    private void ActivateCategory(ShopCategory category) {
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
