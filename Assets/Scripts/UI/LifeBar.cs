using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBar : MonoBehaviour {

    [SerializeField] private Text text;
    [SerializeField] private Slider lifeBar;
    private Player player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

	void Update () {
        if(player != null) {
            lifeBar.value = player.life / player.maxLife;
            text.text = Mathf.Max(player.life, 0) + "/" + player.maxLife;
        }
	}
}
