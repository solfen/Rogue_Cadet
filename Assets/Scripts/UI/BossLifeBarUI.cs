using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLifeBarUI : MonoBehaviour {

    [SerializeField] private List<string> bossesNames;
    [SerializeField] private Animator anim;
    [SerializeField] private Text text;
    [SerializeField] private Slider lifeBar;

    private Enemy bossEnemy;
    private int bossIndex;

    // Use this for initialization
	void Start () {
        EventDispatcher.AddEventListener(Events.BOSS_BECAME_VISIBLE, OnNewBossChallenges);
	}

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.BOSS_BECAME_VISIBLE, OnNewBossChallenges);
        EventDispatcher.RemoveEventListener(Events.BOSS_BEATEN, OnBossBeaten);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnBossBeaten);
    }

    private void OnNewBossChallenges(object bossObj) {
        Boss boss = (Boss)bossObj;
        bossEnemy = boss.GetComponent<Enemy>();
        text.text = bossesNames[boss.bossIndex];

        StartCoroutine("UpdateSliderValue");
        anim.SetTrigger("Open");
        EventDispatcher.AddEventListener(Events.BOSS_BEATEN, OnBossBeaten);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnBossBeaten);
    }

    private void OnBossBeaten(object useless) {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnBossBeaten);
        EventDispatcher.RemoveEventListener(Events.BOSS_BEATEN, OnBossBeaten);
        StopCoroutine("UpdateSliderValue");
        anim.SetTrigger("Close");
    }

    IEnumerator UpdateSliderValue() {
        lifeBar.value = 0;
        yield return new WaitForSecondsRealtime(1f);

        for (float t = 0; t < 0.5f; t += Time.unscaledDeltaTime) {
            lifeBar.value = Mathf.Lerp(0, 1, t / 0.5f);
            yield return null;
        }

        while (bossEnemy.life > 0) {
            lifeBar.value = bossEnemy.life / bossEnemy.maxLife;
            yield return null;
        }
    }
}
