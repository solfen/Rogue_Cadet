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

    public void Open() {
        anim.SetTrigger("Open");
    }

    public void Close() {
        anim.SetTrigger("Close");
    }

    public void UpdateDetails(BaseUpgrade upgrade) {
        header.text = upgrade.title;
        description.text = upgrade.description;
        activeNb.text = "Active: " + upgrade.currentEquipedNb + "/" + upgrade.numberOfUpgrade;
        price.text = "Price: " + (int)upgrade.currentPrice;
        wheight.text = "Wheight: " + upgrade.wheight;
        totalWeight.text = "Tot Wheight: " + (int)(upgrade.wheight * upgrade.currentEquipedNb);
    }
}
