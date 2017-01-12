using UnityEngine;
using System.Collections;

public class AchievementKillXEnemiesWhilePower : BaseAchievement {

    [SerializeField]
    private int deathNb;
    private int currentDeathNb = 0;

    protected override void Start() {
        base.Start();

        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED, OnPowerUsed);
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USE_END, OnPowerUseEnd);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED, OnPowerUsed);
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USE_END, OnPowerUseEnd);
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnPowerUsed(object useless) {
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, OnEnemyDied);
        currentDeathNb = 0;
    }

    private void OnPowerUseEnd(object useless) {
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnEnemyDied(object useless) {
        currentDeathNb++;

        if (currentDeathNb >= deathNb) {
            Unlock();
        }
    }
}