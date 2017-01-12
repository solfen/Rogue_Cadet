using UnityEngine;
using System.Collections;

public class AchievementKilingSpree : BaseAchievement {

    [SerializeField] private float maxKillInterval;
    [SerializeField] private int spreeNeededNb;

    private int spreeNb;
    private float spreeTimer = 0;

    protected override void Start() {
        base.Start();

        EventDispatcher.AddEventListener(Events.ENEMY_DIED, OnEnemyDied);
        StartCoroutine(ResetSpree());
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnEnemyDied(object useless) {
        spreeTimer = 0;
        spreeNb++;

        if (spreeNb >= spreeNeededNb) {
            Unlock();
        }
    }

    IEnumerator ResetSpree() {
        while(true) {
            for(; spreeTimer < maxKillInterval; spreeTimer += Time.unscaledDeltaTime) {
                yield return null;
            }

            spreeTimer = 0;
            spreeNb = 0;
        }
    }
}
