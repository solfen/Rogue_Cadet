using UnityEngine;
using System.Collections;

public class AchievementNotifUI : MonoBehaviour {
    
    [SerializeField] private AchievementUIMain achievementUI;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        EventDispatcher.AddEventListener(Events.ACHIEVMENT_UNLOCKED, OnAchievementUnlocked);
	}

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.ACHIEVMENT_UNLOCKED, OnAchievementUnlocked);
    }

    private void OnAchievementUnlocked(object indexObj) {
        achievementUI.Init(GlobalData.instance.gameData.achievementsUI[(int)indexObj], (int)indexObj);
        EventDispatcher.DispatchEvent(Events.OPEN_UI_PANE, null);
        anim.SetTrigger("Open");
    }

}
