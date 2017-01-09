using UnityEngine;
using System.Collections;

public abstract class BaseAchievement : MonoBehaviour {

    [SerializeField] private int achievementIndex;
    [SerializeField] private int shipToUnlock = -1;

	// Use this for initialization
	protected virtual void Start () {
        EventDispatcher.DispatchEvent(Events.ACHIEVMENT_CREATED, achievementIndex);

	    if(GlobalData.instance.saveData.achievementsUnlocked.Contains(achievementIndex)) {
            Destroy(this);
            return;
        }
	}

    protected virtual void Unlock() {
        SaveData data = FileSaveLoad.Load();
        data.achievementsUnlocked.Add(achievementIndex);
        if (shipToUnlock != -1)
            data.shipsInfo[shipToUnlock].isUnlocked = true;

        FileSaveLoad.Save(data);

        EventDispatcher.DispatchEvent(Events.ACHIEVMENT_UNLOCKED, achievementIndex);

        Destroy(this);
    }
}
