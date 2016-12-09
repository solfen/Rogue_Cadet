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

        float maxWheight = gameData.ships[saveData.selectedShip].maxWheightPercent * gameData.shipBaseStats.maxWeight;
        wheightText.text = ((int)saveData.shipWeight) + " / " + ((int)maxWheight);
        slider.value = saveData.shipWeight / maxWheight;
    }
}
