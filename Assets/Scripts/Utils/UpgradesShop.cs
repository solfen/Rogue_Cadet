using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

public class UpgradesShop : MonoBehaviour {

    [SerializeField] private List<ShopCategory> itemsCategories = new List<ShopCategory>();
    [SerializeField] private ShopUI shopUI;

    private int currentCategory;

    // Use this for initialization
    void Start () {
        shopUI.InitItems(itemsCategories);
    }

    void Update() {
        if (Input.GetButtonDown("Start")) { //weird that it's here...
            InputMapUI.instance.Open();
        }
    }

    //called from UI
    public void OnCategorySelected(int index) {
        shopUI.OpenShop(itemsCategories[index]);
    }

    public void BuyItem(int index) {
        ShopItem item = itemsCategories[currentCategory].items[index];
        /*float price = 
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
        }*/
    }
}
