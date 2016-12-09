using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSelector : MonoBehaviour {

    [SerializeField] private ShipSelectionTransitionUI transistor;

    void Start () {
        SaveData data = FileSaveLoad.Load();
        List<ShipConfig> ships = GlobalData.instance.gameData.ships;

        for (int i = 0; i < ships.Count; i++) {
            data.shipsStock[i] = Mathf.Min(ships[i].maxStock, data.shipsStock[i] + ships[i].stockGainByRun);
        }

        FileSaveLoad.Save(data);
    }

    //called from shipDetails pane (weird, I know)
    public void SelectShip(int shipIndex) {
        SaveData data = FileSaveLoad.Load();

        if(data.shipsStock[shipIndex] >= 1) {
            data.selectedShip = shipIndex;
            data.shipsStock[shipIndex] -= 1;
            FileSaveLoad.Save(data);

            transistor.Transition();
        }
        // TODO: Else sound of error 
    }
}
