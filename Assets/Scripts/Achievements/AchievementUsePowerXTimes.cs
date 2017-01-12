using UnityEngine;
using System.Collections;

public class AchievementUsePowerXTimes : BaseAchievement {

    [SerializeField] private int useNb;
    private int currentUseNb = 0;

	protected override void Start () {
        base.Start();
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED, OnPowerUsed);
	}

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED, OnPowerUsed);
    }

    private void OnPowerUsed(object useless) {
        currentUseNb++;

        if(currentUseNb >= useNb) {
            Unlock();
        }
    }
}
