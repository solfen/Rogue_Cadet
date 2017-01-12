using UnityEngine;
using System.Collections;

public class AchievementKillInLimitedTime : BaseAchievement {

    [SerializeField] private float totalSeconds;
    [SerializeField] private int killsNeededNb;

    private int killsNb;

    protected override void Start() {
        base.Start();

        EventDispatcher.AddEventListener(Events.ENEMY_DIED, OnEnemyDied);
        StartCoroutine(WaitForDeadline());
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnEnemyDied(object useless) {
        killsNb++;
        if (killsNb >= killsNeededNb) {
            Unlock();
        }
    }

    IEnumerator WaitForDeadline() {
        yield return new WaitForSecondsRealtime(totalSeconds);
        Destroy(this);
    }
}
