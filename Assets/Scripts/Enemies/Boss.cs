using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    public int bossNb;
    public Enemy enemyScriptRef;
	
	// Update is called once per frame
	void Update () {
	    if(enemyScriptRef.life <= 0) {
            PlayerPrefs.SetInt("Defeated_Boss" + bossNb, 1);
        }
	}
}
