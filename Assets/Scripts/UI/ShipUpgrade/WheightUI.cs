using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WheightUI : MonoBehaviour {

    [SerializeField] private Text wheightText;
    [SerializeField] private Slider slider;
	// Use this for initialization
	void Start () {
        UpdateWheight();
    }

    public void UpdateWheight() {
        GameData gameData = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;

        float maxWheight = gameData.shipBaseStats.maxWeight * (gameData.ships[saveData.selectedShip].maxWheightPercent + saveData.wheightUpgradeNb * gameData.ships[saveData.selectedShip].wheightUpgradeRaise);
        wheightText.text = ((int)saveData.shipWeight) + " / " + ((int)maxWheight);
        slider.value = saveData.shipWeight / maxWheight;
    }
}
