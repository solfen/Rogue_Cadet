using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {

    [SerializeField] private Dungeon dungeon;

	void Start () {
        Player player = Instantiate(GlobalData.instance.gameData.ships[GlobalData.instance.saveData.selectedShip].playerPrefab) as Player;
        player.Init(dungeon);
    }
}
