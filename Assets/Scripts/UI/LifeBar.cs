using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBar : MonoBehaviour {

    [SerializeField] private GameData gameData;
    [SerializeField] private Text text;
    [SerializeField] private Slider lifeBar;

    private Player player;

    void Awake() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        enabled = false;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
    }

    private void OnPlayerCreation(object playerObj) {
        player = (Player)playerObj;
        enabled = true;
    }


    void Update () {
        if(player != null) {
            lifeBar.value = player.currentLife / player.maxLife;
            text.text = Mathf.Max((int)(player.currentLife), 0) + "/" + ((int)player.maxLife);
        }
	}
}
