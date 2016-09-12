using UnityEngine;
using System.Collections;

public class MovementSwitching : MonoBehaviour {

    public Transform target;
    public float detectionDistance;
    public float lostSightDistance;
    public BaseMovement idleMovement;
    public BaseMovement attackMovement;
    public Weapon weapon;

    private Transform _transform;
    private bool isIdle = true;
    private float targetDistance;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        attackMovement.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (target == null) {
            return;
        }

        targetDistance = Vector3.Distance(_transform.position, target.position);

        if(targetDistance < detectionDistance && isIdle) {
            isIdle = false;
            attackMovement.enabled = true;
            idleMovement.enabled = false;
            weapon.enabled = true;
        }
        else if(targetDistance > lostSightDistance && !isIdle) {
            isIdle = true;
            attackMovement.enabled = false;
            idleMovement.enabled = true;
            weapon.enabled = false;
        }
    }
}
