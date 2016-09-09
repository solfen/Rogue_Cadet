using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class LifeBar : MonoBehaviour {

    public Player player;

    private Text text;
    void Start() {
        text = GetComponent<Text>();
    }
	// Update is called once per frame
	void Update () {
        if(player != null) {
	        text.text = "Life: " + Mathf.Max(player.life, 0) + "/" + player.maxLife;
        }
	}
}
