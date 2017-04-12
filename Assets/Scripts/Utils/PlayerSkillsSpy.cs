using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillsSpy : MonoBehaviour {

    [SerializeField] private float normalTimeBeforeFirstKill;
    [SerializeField] private float maxTimeDifference;
    [SerializeField] private float maxScore;

    private float timeStart;
    private float timePlayedBeforeFirstKill = 0;
    private float enemiesKilledScore;

	// Use this for initialization
	void Start () {
        timeStart = Time.time;

        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnPlayerDeath(object useless) {
        timePlayedBeforeFirstKill = timePlayedBeforeFirstKill == 0 ? Time.time - timeStart : timePlayedBeforeFirstKill;
        float skillRatio = (enemiesKilledScore / maxScore + Mathf.Min(normalTimeBeforeFirstKill / timePlayedBeforeFirstKill, 1)) / 2;

        PlayerPrefs.SetFloat("PlayerLifeMultiplier", Mathf.Lerp(2f, 0.6f, skillRatio));
        PlayerPrefs.SetFloat("GoldMultiplier", Mathf.Lerp(1f, 1.7f, Mathf.Abs((normalTimeBeforeFirstKill - timePlayedBeforeFirstKill) / maxTimeDifference)));
        PlayerPrefs.SetFloat("EnemiesLifeMultiplier", Mathf.Lerp(0.2f, 1.33f, skillRatio));
        PlayerPrefs.SetFloat("EnemiesBulletsDmgMultiplier", Mathf.Lerp(0.22f, 1.4f, skillRatio));
        PlayerPrefs.SetFloat("EnemiesFireSpeedMultiplier", Mathf.Lerp(0.6f, 1.3f, skillRatio));

        EventDispatcher.DispatchEvent(Events.PLAYER_SKILLS_SNIFFED, null);
    }

    private void OnEnemyDied(object enemyObj) {
        Enemy enemy = (Enemy)enemyObj;
        enemiesKilledScore += enemy.score;

        if(enemy.score > 0 && timePlayedBeforeFirstKill == 0) { // fake tuto enemies have a score of 0
            timePlayedBeforeFirstKill = Time.time - timeStart;
        }
    }
}
