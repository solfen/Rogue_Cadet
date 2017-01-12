using UnityEngine;
using System.Collections;

public class AchievementKillWithMinLife : BaseAchievement {

    [SerializeField] private float minLifePercent;
    [SerializeField] private int killsNeededNb;

    private int killsNb;

    protected override void Start() {
        base.Start();

        EventDispatcher.AddEventListener(Events.PLAYER_HIT, OnPlayerHit);
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_HIT, OnPlayerHit);
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, OnEnemyDied);
    }

    private void OnPlayerHit(object playerObj) {
        Player player = (Player)playerObj;
        if (player.currentLife < player.maxLife * minLifePercent) {
            Destroy(this);
        }
    }

    private void OnEnemyDied(object useless) {
        killsNb++;
        if(killsNb >= killsNeededNb) {
            Unlock();
        }
    }
}
