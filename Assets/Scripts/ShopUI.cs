using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class ShopItem {
    public Sprite image;
    public string title;
    public float price;
    public int numberOfUpgrade;
    public float priceMultiplierPerUpgrade;
    public string saveKey;
    public int saveValue;
    public bool isChoice;
    [HideInInspector]
    public int currentValue;
}

[System.Serializable]
public class ShopItemUI {
    public Image render;
    public Text title;
    public Text price;
    public Text number;
}

[System.Serializable]
public class ShopCategory {
    public List<ShopItem> items;
}

public class ShopUI : MonoBehaviour {

    public Animator anim;
    public List<ShopCategory> items = new List<ShopCategory>();
    public ShopItemUI[] itemsUI = new ShopItemUI[3];
    public EventSystem eventSystem;
    public GameObject firstShopItem;
    public GameObject firstCategoryItem;
    public Text moneyText;

    private int currentCategory;
    private float money;
    private bool isOpen;

    void Start() {
        money = PlayerPrefs.GetFloat("Money", 6000);
        moneyText.text = "Money: " + money + "$";
    }

    void Update() {
        if(isOpen && Input.GetButtonDown("Cancel")) {
            CloseShop();
        }
        if(Input.GetButtonDown("Restart")) {
            Application.LoadLevel(0);
        }
    }

    public void OpenShop(int category) { 
        eventSystem.SetSelectedGameObject(firstShopItem);

        currentCategory = category;
        UpdateItems();
        anim.SetTrigger("OpenShop");

        isOpen = true;
    }

    private void CloseShop() {
        eventSystem.SetSelectedGameObject(firstCategoryItem);
        anim.SetTrigger("CloseShop");
        isOpen = false;
    }

    private void UpdateItems() {
        for (int i = 0; i < 3; i++) {
            ShopItem item = items[currentCategory].items[i];
            item.currentValue = PlayerPrefs.GetInt(item.saveKey, 0);

            itemsUI[i].render.sprite = item.image;
            itemsUI[i].title.text = item.title;
            itemsUI[i].price.text = item.isChoice ? item.currentValue == item.saveValue ? 0 + "$" : (int)item.price + "$" : (int)(item.price * Mathf.Pow(item.currentValue+1, item.priceMultiplierPerUpgrade)) + "$";
            itemsUI[i].number.text = item.isChoice ? item.currentValue == item.saveValue ? "Selected" : ""  : "Upgrade: " + item.currentValue + "/" + item.numberOfUpgrade;
        }
    }

    public void BuyItem(int index) {
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
            moneyText.text = "Money: " + money + "$";
            UpdateItems();
        }
    }

}
