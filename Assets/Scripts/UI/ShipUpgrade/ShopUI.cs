using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class ShopItem {
    public Sprite sprite;
    public string title;
    public string description;
    public float price;
    public int numberOfUpgrade;
    public float wheight;
}

[System.Serializable]
public class ShopCategory {
    public List<ShopItem> items;
}

public class ShopUI : MonoBehaviour {

    [SerializeField] private ShopDetailsUI shopDetailPane;
    [SerializeField] private GameObject categroryContainerPrefab;
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private List<ShopCategory> itemsCategories = new List<ShopCategory>();
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

    void Start() {
        _transform = GetComponent<Transform>();
        gridLayout = GetComponent<ResponsiveGridLayout>();
        //money = PlayerPrefs.GetFloat("Money", 0);
        //moneyText.text = "Money: " + Mathf.Floor(money) + " $";

        for (int i = 0; i < itemsCategories.Count; i++) {
            GameObject container = Instantiate(categroryContainerPrefab, _transform, false) as GameObject;
            Transform containerTransform = container.GetComponent<Transform>();
            RectTransform containerRectTrans = container.GetComponent<RectTransform>();
            containerRectTrans.anchorMin = Vector2.zero;
            containerRectTrans.anchorMax = new Vector2(1,1);

            for(int j = 0; j < itemsCategories[i].items.Count; j++) {
                GameObject item = Instantiate(shopItemPrefab, containerTransform, false) as GameObject;
                item.GetComponent<ShopItemUI>().UpdateItem(itemsCategories[i].items[j], j, shopDetailPane);
            }

            container.SetActive(false);
        }
    }

    void Update() {
        if(isOpen && Input.GetButtonDown("Cancel")) {
            CloseShop();
        }
        if(Input.GetButtonDown("Start")) { //weird that it's here...
            InputMapUI.instance.Open();
        }
    }

    public void OpenShop(int category) {
        lastCategorySelected = eventSystem.currentSelectedGameObject;
        currentCategory = category;

        shopDetailPane.Open(itemsCategories[currentCategory]); //needs to be before ActivateCategory because selecting the first object calls an update on the details (berk)
        anim.SetTrigger("OpenShop");

        ActivateCategory();

        isOpen = true;
    }

    private void CloseShop() {
        eventSystem.SetSelectedGameObject(lastCategorySelected);
        anim.SetTrigger("CloseShop");
        shopDetailPane.Close();
        isOpen = false;
    }

    private void ActivateCategory() {
        if(activeCategoryContainer != null) {
            activeCategoryContainer.SetActive(false);
        }
        activeCategoryContainer = _transform.GetChild(currentCategory).gameObject;
        activeCategoryContainer.SetActive(true);

        gridLayout.target = activeCategoryContainer.transform;
        gridLayout.Resize();

        eventSystem.SetSelectedGameObject(activeCategoryContainer.transform.GetChild(0).gameObject);
    }

    /*public void BuyItem(int index) {
        ShopItem item = items[currentCategory].items[index];
        float price = item.isChoice ? item.currentValue == item.saveValue ? 0 : item.price : item.price * Mathf.Pow(item.currentValue+1, item.priceMultiplierPerUpgrade);
        if (money >= price) {
            if(item.isChoice) {
                money -= price;
                item.price = 0; //won't be saved
                PlayerPrefs.SetInt(item.saveKey, item.saveValue);
            }
            else if(item.currentValue < item.numberOfUpgrade) {
                money -= price;
                item.currentValue++;
                PlayerPrefs.SetInt(item.saveKey, item.currentValue);
            }

            PlayerPrefs.SetFloat("Money", money);
            moneyText.text = "Money: " + Mathf.Floor(money) + "$";
            UpdateItems();
        }
    }*/

}
