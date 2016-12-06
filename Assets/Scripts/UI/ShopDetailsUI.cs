using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopDetailsUI : MonoBehaviour {

    [SerializeField] private Animator anim;
    [SerializeField] private Text header;
    [SerializeField] private Text description;
    [SerializeField] private Text activeNb;
    [SerializeField] private Text price;
    [SerializeField] private Text wheight;
    [SerializeField] private Text totalWeight;

    private ShopCategory currentCategory;

    public void Open(ShopCategory category) {
        currentCategory = category;
        UpdateDetails(0);

        anim.SetTrigger("Open");
    }

    public void Close() {
        anim.SetTrigger("Close");
    }

    public void UpdateDetails(int selectedItem) {
        ShopItem data = currentCategory.items[selectedItem];
        header.text = data.title;
        description.text = data.description;
        activeNb.text = "Active: 0/" + data.numberOfUpgrade;
        price.text = "Price: " + data.price;
        wheight.text = "Wheight: " + data.wheight; 
    }
}
