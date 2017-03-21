using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossWantedUI : MonoBehaviour {

    [SerializeField] private int bossIndex;
    [SerializeField] private Image icon;
    [SerializeField] private float fadeDuration = 1;

	void Start () {
		if(GlobalData.instance.saveData.bossesBeaten.Contains(bossIndex)) {
            icon.gameObject.SetActive(true);
        }
        else {
            EventDispatcher.AddEventListener(Events.BOSS_BEATEN, OnBossBeaten);
        }
	}

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.BOSS_BEATEN, OnBossBeaten);
    }

    private void OnBossBeaten(object bossIndexObj) {
        if( (int)bossIndexObj == bossIndex) {
            StartCoroutine(IconAppearFade());
        }
    }

    IEnumerator IconAppearFade() {
        yield return new WaitForSecondsRealtime(1.65f); //boss explosion time
        icon.gameObject.SetActive(true);
        icon.CrossFadeAlpha(0, 0, true);
        icon.CrossFadeAlpha(1, fadeDuration, true);
    }
}
