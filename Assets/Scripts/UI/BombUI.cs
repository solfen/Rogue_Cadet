using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour {

    [SerializeField] private Text bombText;

    // Use this for initialization
    void Awake () {
        EventDispatcher.AddEventListener(Events.BOMB_USED, OnUsePower);
	}

    void Start() {
        if (GlobalData.instance.saveData.bombUpgradeNb == 0) {
            Destroy(gameObject);
        }
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.BOMB_USED, OnUsePower);
    }

    private void OnUsePower(object bombObj) {
        Bomb bomb = (Bomb)bombObj;
        bombText.text = LocalizationManager.GetLocalizedText("GAME_UI_BOMBS") + bomb.currentStock + "/" + bomb.maxStock;
    }
}
