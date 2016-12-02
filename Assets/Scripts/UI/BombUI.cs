using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour {

    [SerializeField] private Text bombText;

    // Use this for initialization
    void Awake () {
        /*if(!PlayerPrefs.HasKey("Equiped_Bomb")) {
            gameObject.SetActive(false);
            return;
        }*/

        EventDispatcher.AddEventListener(Events.BOMB_USED, OnUsePower);
	}

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.BOMB_USED, OnUsePower);
    }

    private void OnUsePower(object bombObj) {
        Bomb bomb = (Bomb)bombObj;
        Debug.Log("WUT");
        bombText.text = "Bomb stock: " + bomb.currentStock + "/" + bomb.maxStock;
    }

}
