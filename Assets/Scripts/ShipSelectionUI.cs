using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionUI : MonoBehaviour {

    public Test test;

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

        test.test = "caca";
    }
	
    public void SelectShip(string name) {
        selectedShip = ships[name];
        statText.text = "Health: " + selectedShip.maxLife + " / Attack: x" + selectedShip.GetComponentInChildren<Weapon>().damageMultiplier;
    }

    public void ValidateSelection() {
        PlayerPrefs.SetString("selectedShip", selectedShip.typeName);
        transitionAnimator.SetTrigger("Transition");
    }
}
