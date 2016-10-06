using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

    [SerializeField]
    private List<Player> shipTypesList;

    void Awake () {
        if(GameObject.FindGameObjectWithTag("Player") == null) {
            string currentType = PlayerPrefs.GetString("selectedShip", "Knight");
            for (int i = 0; i < shipTypesList.Count; i++) {
                if(shipTypesList[i].typeName == currentType) {
                    Instantiate(shipTypesList[i]);
                }
            }
        }
    }
}
