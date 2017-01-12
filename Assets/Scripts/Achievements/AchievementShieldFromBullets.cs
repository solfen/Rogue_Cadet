using UnityEngine;
using System.Collections;

public class AchievementShieldFromBullets : BaseAchievement {

    [SerializeField] private int bulletsToAbsorb = 5;
    private int bulletAbsorbed;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED, OnSpecialPowerUsed);
        EventDispatcher.AddEventListener(Events.SHIELD_ABSORB_BULLET, OnShieldAbsorb);
	}

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED, OnSpecialPowerUsed);
        EventDispatcher.RemoveEventListener(Events.SHIELD_ABSORB_BULLET, OnShieldAbsorb);
    }

    private void OnSpecialPowerUsed(object useless) {
        bulletAbsorbed = 0;
    }

    void OnShieldAbsorb(object useless) {
        bulletAbsorbed++;
        if(bulletAbsorbed == bulletsToAbsorb) {
            Unlock();
        }
    }
}
