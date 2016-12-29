using UnityEngine;
using System.Collections;

public class PowerReavealTreasures : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        EventDispatcher.AddEventListener(Events.GAME_LOADED, OnGameLoaded);
	}

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.GAME_LOADED, OnGameLoaded);
    }
    
    private void OnGameLoaded(object useless) {
        EventDispatcher.DispatchEvent(Events.REVEAL_TREASURE_MAP, null);
    }
}
