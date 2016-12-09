using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBar : MonoBehaviour {

    [SerializeField] private GameData gameData;
    [SerializeField] private Text text;
    [SerializeField] private Slider lifeBar;

    private Player player;
    private float maxLife;

    void Start() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        enabled = false;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
    }

    private void OnPlayerCreation(object playerObj) {
        player = (Player)playerObj;
        maxLife = player.maxLife;
        enabled = true;
    }


    void Update () {
        if(player != null) {
            lifeBar.value = player.currentLife / maxLife;
            text.text = Mathf.Max((int)(player.currentLife), 0) + "/" + maxLife;
        }
	}
}
