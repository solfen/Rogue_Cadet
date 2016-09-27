using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BehaviourSwitch {
    public Behaviour script;
    public bool inIdle;
    public bool inAttack;
}

public class BehaviourSwitching : MonoBehaviour {

    public float reactionDuration = 1f;
    public List<BehaviourSwitch> behavioursToSwitch;

    private Transform _transform;
    private bool isIdle = true;
    private float reactionTimer;
    private bool isReactionStarted = false;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();

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

        if (!isReactionStarted && gameObject.activeSelf) {
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
            behavioursToSwitch[i].script.enabled = (behavioursToSwitch[i].inIdle && isIdle) || (behavioursToSwitch[i].inAttack && !isIdle);
        }
    }

}
