using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BehaviourLifeSwitch {
    public Behaviour script;
    [Tooltip("inclusive")] [Range(0,1)] public float minLifePercent;
    [Tooltip("exclusive")] [Range(0,1)] public float maxLifePercent;
    [HideInInspector] public float minLife;
    [HideInInspector] public float maxLife;
}

[RequireComponent(typeof(Enemy))]
public class BehaviourLifeSwitching : MonoBehaviour {

    public List<BehaviourLifeSwitch> behavioursToSwitch;
    private Enemy bearer;
	// Use this for initialization
	void Start () {
        bearer = GetComponent<Enemy>();
        for (int i = 0; i < behavioursToSwitch.Count; i++) {
            behavioursToSwitch[i].minLife = bearer.life * behavioursToSwitch[i].minLifePercent;
            behavioursToSwitch[i].maxLife = bearer.life * behavioursToSwitch[i].maxLifePercent;
        }
    }

    // Update is called once per frame
    void Update () {
	    for(int i = 0; i < behavioursToSwitch.Count; i++) {
            ((ISwitchable)behavioursToSwitch[i].script).SwitchState(bearer.life >= behavioursToSwitch[i].minLife && bearer.life < behavioursToSwitch[i].maxLife);
        }
	}
}
