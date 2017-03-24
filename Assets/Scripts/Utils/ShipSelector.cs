using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSelector : MonoBehaviour {

    [SerializeField] private ShipSelectionTransitionUI transitor;

    void Start () {
        SaveData data = FileSaveLoad.Load();
        List<ShipConfig> ships = GlobalData.instance.gameData.ships;

        for (int i = 0; i < ships.Count; i++) {
            if(data.selectedShip != i) {
                data.shipsInfo[i].stock = Mathf.Min(ships[i].maxStock, data.shipsInfo[i].stock + ships[i].stockGainByRun);
            }
        }

        FileSaveLoad.Save(data);
    }

    //called from shipDetails pane (weird, I know)
    public void SelectShip(int shipIndex) {
        SaveData data = FileSaveLoad.Load();

        if(data.shipsInfo[shipIndex].stock >= 1 && data.shipsInfo[shipIndex].isUnlocked) {
            data.selectedShip = shipIndex;
            data.shipsInfo[shipIndex].stock -= 1;
            FileSaveLoad.Save(data);

            transitor.Transition();
        }
    }
}
