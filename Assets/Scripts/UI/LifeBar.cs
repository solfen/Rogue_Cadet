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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxLife = gameData.shipBaseStats.maxLife * gameData.ships[PlayerPrefs.GetInt("SelectedShip", 0)].lifePrecent;
    }

	void Update () {
        if(player != null) {
            lifeBar.value = player.currentLife / maxLife;
            text.text = Mathf.Max((int)(player.currentLife), 0) + "/" + maxLife;
        }
	}
}
