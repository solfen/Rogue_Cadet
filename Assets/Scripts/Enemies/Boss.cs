using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour {

    [SerializeField] private int bossIndex;
    [SerializeField] private Enemy enemyScriptRef;
    [SerializeField] private GameObject bossBeatenObjects;

    private bool isDead = false;
	
    void Start () {
        bossBeatenObjects.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
	    if(!isDead && enemyScriptRef.life <= 0) {
            BossDied();
            isDead = true;
            enabled = false;
        }
	}

    private void BossDied() {
        bossBeatenObjects.SetActive(true);

        SaveData data = FileSaveLoad.Load();
        data.bossesBeaten.Add(bossIndex);
        FileSaveLoad.Save(data);

        EventDispatcher.DispatchEvent(Events.BOSS_BEATEN, bossIndex);
    }


}
