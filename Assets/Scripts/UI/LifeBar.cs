using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class LifeBar : MonoBehaviour {

    private Text text;
    private Player player;

    void Start() {
        text = GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

	void Update () {
        if(player != null) {
	        text.text = "Life: " + Mathf.Max(player.life, 0) + "/" + player.maxLife;
        }
	}
}
