using UnityEngine;
using System.Collections;

public class AchievementsListUI : MonoBehaviour {

    [SerializeField] private AchievementUIMain UIprefab;
	// Use this for initialization
	void Start () {
        EventDispatcher.AddEventListener(Events.ACHIEVMENT_CREATED, OnAchievementCreated);
	}

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.ACHIEVMENT_CREATED, OnAchievementCreated);
    }

    private void OnAchievementCreated(object achievementIndex) {
        int i = (int)achievementIndex;
        AchievementUIMain element = Instantiate(UIprefab, transform, false) as AchievementUIMain;
        element.Init(GlobalData.instance.gameData.achievementsUI[i], GlobalData.instance.saveData.achievementsUnlocked.Contains(i));
    }
}
