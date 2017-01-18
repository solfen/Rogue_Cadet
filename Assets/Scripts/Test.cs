using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    public Text realText;

    void Start () {
        StartCoroutine("Teste");
    }
	
    IEnumerator Teste() {
        yield return new WaitForSeconds(1);
        EventDispatcher.DispatchEvent(Events.GAME_STARTED, null);
    }
}
