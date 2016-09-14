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

    public float detectionDistance;
    public float lostSightDistance;

    public List<BehaviourSwitch> behavioursToSwitch;

    private Transform _transform;
    private Transform player;
    private bool isIdle = true;
    private float targetDistance;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SwitchBehaviour();
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null) {
            return;
        }

        targetDistance = Vector3.Distance(_transform.position, player.position);

        if(targetDistance < detectionDistance && isIdle) { // could probably set the isIdle in one line but it'll be more complicated, and not really more optimized
            isIdle = false;
            SwitchBehaviour();
        }
        else if(targetDistance > lostSightDistance && !isIdle) {
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
