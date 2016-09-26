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

    public float reactionTime = 1f;
    public List<BehaviourSwitch> behavioursToSwitch;

    private Transform _transform;
    private Transform player;
    private bool isIdle = true;
    private float targetDistance;
    private Renderer render;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        render = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        SwitchBehaviour();
    }
	
    void OnBecameVisible () {
        StopCoroutine("FoundTarget"); //TODO: Check if having a timer in Update is more optimized
        StartCoroutine("FoundTarget");
    }

    void OnBecameInvisible () {
        StopCoroutine("LostTarget");
        StartCoroutine("LostTarget");
    }

    IEnumerator FoundTarget() {
        yield return new WaitForSeconds(reactionTime);
        if (render.isVisible) {
            isIdle = false;
            SwitchBehaviour();
        }
    }

    IEnumerator LostTarget() {
        yield return new WaitForSeconds(reactionTime);
        if(!render.isVisible) {
            isIdle = true;
            SwitchBehaviour();
        }
    }

    private void SwitchBehaviour() {
        for(int i = 0; i < behavioursToSwitch.Count; i++) {
            behavioursToSwitch[i].script.enabled = (behavioursToSwitch[i].inIdle && isIdle) || (behavioursToSwitch[i].inAttack && !isIdle);
        }
    }

}
