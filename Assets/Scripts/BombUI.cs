using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour {

    public static BombUI instance;

    [SerializeField]
    private Text bombText;

    // Use this for initialization
    void Awake () {
        instance = this;
	}

    public void OnUsePower(SpecialPower bomb) {
        bombText.text = "Bomb stock: " + bomb.mana + "/" + bomb.maxMana;
    }

}
