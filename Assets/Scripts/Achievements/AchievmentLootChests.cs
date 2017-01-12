using UnityEngine;
using System.Collections;

public class AchievmentLootChests : BaseAchievement {

    [SerializeField] private int chestNeedNb;
    private int chestLooted;

	protected override void Start () {
        base.Start();

        EventDispatcher.AddEventListener(Events.CHEST_LOOTED, OnChestLooted);
	}
	
    private void OnChestLooted(object useless) {
        chestLooted++;
        if(chestLooted >= chestNeedNb) {
            Unlock();
        }
    }
}
