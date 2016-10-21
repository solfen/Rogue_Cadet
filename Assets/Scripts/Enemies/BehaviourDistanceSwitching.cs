using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BehaviourDistanceSwitch {
    public Behaviour script;
    public bool inIdle;
    public bool inRange;
}

public class BehaviourDistanceSwitching : MonoBehaviour {
    public float detectionDistance = 15;
    public float reactionDuration = 1f;
    public List<BehaviourDistanceSwitch> behavioursToSwitch;

    private bool isIdle = true;
    private float reactionTimer;
    private bool isReactionStarted = false;
    private Transform player;
    private Transform _transform;

    void Awake() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        enabled = false;
    }

    void Start() {
        _transform = GetComponent<Transform>();
        SwitchBehaviour();
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    private void OnPlayerCreation(object _player) {
        player = ((Player)_player).GetComponent<Transform>();
        enabled = true;
    }

    private void OnPlayerDeath(object useless) {
        enabled = false;
    }

    void Update () {
        bool newState = Vector3.Distance(_transform.position, player.position) > detectionDistance;
        if(isIdle != newState) {
            isIdle = newState;
            reactionTimer = reactionDuration;

            if (!isReactionStarted && gameObject.activeInHierarchy) {
                StartCoroutine(ReactionDelay());
            }
        }

        //DrawRay(_transform.position, (player.position - _transform.position).normalized * detectionDistance, Color.red);
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
        for (int i = 0; i < behavioursToSwitch.Count; i++) {
            behavioursToSwitch[i].script.enabled = (behavioursToSwitch[i].inIdle && isIdle) || (behavioursToSwitch[i].inRange && !isIdle);
        }
    }

}
