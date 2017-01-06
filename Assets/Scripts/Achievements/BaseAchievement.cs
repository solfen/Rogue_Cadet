using UnityEngine;
using System.Collections;

public abstract class BaseAchievement : MonoBehaviour {

    [SerializeField] private int achievementIndex;

	// Use this for initialization
	protected virtual void Start () {
	    if(GlobalData.instance.saveData.achievementsUnlocked.Contains(achievementIndex)) {
            Destroy(this);
            return;
        }
	}

    protected virtual void Unlock() {
        SaveData data = FileSaveLoad.Load();
        data.achievementsUnlocked.Add(achievementIndex);
        FileSaveLoad.Save(data);

        Debug.Log("Achievement: " + achievementIndex + " unlcoked");
        //UI Event

        Destroy(this);
    }
}
