using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    [SerializeField] private int bossIndex;
    [SerializeField] private Enemy enemyScriptRef;
    [SerializeField] private GameObject bossBeatenObjects;
	
    void Start () {
        bossBeatenObjects.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
	    if(enemyScriptRef.life <= 0) {
            BossDied();
        }
	}

    private void BossDied() {
        bossBeatenObjects.SetActive(true);

        SaveData data = FileSaveLoad.Load();
        data.bossesBeaten.Add(bossIndex);
        FileSaveLoad.Save(data);
    }
}
