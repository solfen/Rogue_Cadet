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
        if(upgrade.isUnlocked) {
            header.text = LocalizationManager.GetLocalizedText(upgrade.title);
            description.text = LocalizationManager.GetLocalizedText(upgrade.description);
            activeNb.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_ACTIVE") + upgrade.currentEquipedNb + "/" + upgrade.numberOfUpgrade;
            price.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_PRICE") + (int)upgrade.currentPrice + "$";
            wheight.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_WHEIGHT") + upgrade.wheight;
            totalWeight.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_TOTAL_WHEIGHT") + (int)(upgrade.wheight * upgrade.currentEquipedNb);
        }
        else {
            header.text = "?????";
            description.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_LOCKED_DESCRIPTION");
            activeNb.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_ACTIVE") + LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_ACTIVE_NONE");
            price.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_PRICE") + "??";
            wheight.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_WHEIGHT") + "???";
            totalWeight.text = LocalizationManager.GetLocalizedText("SHIPS_UPGRADES_TOTAL_WHEIGHT") + "0";
        }
    }
}
