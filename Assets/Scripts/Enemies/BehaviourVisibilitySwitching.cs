using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BehaviourVisibleSwitch {
    public Behaviour script;
    public bool inIdle;
    public bool inVisible;
}

public class BehaviourVisibilitySwitching : MonoBehaviour {

    public float reactionDuration = 1f;
    public List<BehaviourVisibleSwitch> behavioursToSwitch;

    private bool isIdle = true;
    private float reactionTimer;
    private bool isReactionStarted = false;

	// Use this for initialization
	void Start () {
        SwitchBehaviour();
    }
	
    void OnBecameVisible () {
        VisibilityChanged(true);
    }

    void OnBecameInvisible () {
        VisibilityChanged(false);
    }

    private void VisibilityChanged(bool isVisible) {
        isIdle = !isVisible;
        reactionTimer = reactionDuration;

        if (!isReactionStarted && gameObject.activeInHierarchy) {
            StartCoroutine(ReactionDelay());
        }
    }

    IEnumerator ReactionDelay() {
        isReactionStarted = true;

        while (reactionTimer > 0) {
            reactionTimer -= Time.deltaTime;
            yield return null;
        }

        SwitchBehaviour();
        isReactionStarted = false;
    }

    private void SwitchBehaviour() {
        for(int i = 0; i < behavioursToSwitch.Count; i++) {
            ((ISwitchable)behavioursToSwitch[i].script).SwitchState((behavioursToSwitch[i].inIdle && isIdle) || (behavioursToSwitch[i].inVisible && !isIdle));
        }
    }

}
