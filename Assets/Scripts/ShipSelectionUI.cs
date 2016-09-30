using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSelectionUI : MonoBehaviour {

    public GameObject firstUpgradeItem;
    public EventSystem eventSystem;
    public ShopUI shopUI;

    [SerializeField]
    private Animator transitionAnimator;
    [SerializeField]
    private List<Player> shipTypesList;
    [SerializeField]
    private Text statText;
    //private Text specialText;

    private Player selectedShip;

    private Dictionary<string, Player> ships = new Dictionary<string, Player>();
	// Use this for initialization
	void Start () {
        for (int i = 0; i < shipTypesList.Count; i++) {
            ships.Add(shipTypesList[i].typeName, shipTypesList[i]);
        }
    }
	
    public void SelectShip(string name) {
        selectedShip = ships[name];
        statText.text = "Health: " + selectedShip.maxLife + "% / Damage: " + ((int)selectedShip.damageMultiplier*100) + "% / Mana: " + selectedShip.maxMana + "%";
    }

    public void ValidateSelection() {
        shopUI.enabled = true;
        PlayerPrefs.SetString("selectedShip", selectedShip.typeName);
        transitionAnimator.SetTrigger("Transition");
        eventSystem.SetSelectedGameObject(firstUpgradeItem);
    }
}
