using UnityEngine;
using System.Collections;

public class AchievmentMakeMoney : BaseAchievement {

	[SerializeField] private int scoreNeeded;

	protected override void Start () {
        base.Start();

        EventDispatcher.AddEventListener(Events.SCORE_CHANGED, OnScoreChanged);
	}

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.SCORE_CHANGED, OnScoreChanged);
    }

    private void OnScoreChanged(object scoreObj) {
        if((float)scoreObj >= scoreNeeded) {
            Unlock();
        }
    }
}
