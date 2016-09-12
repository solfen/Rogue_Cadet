using UnityEngine;
using System.Collections;

public class TargetMovement : MonoBehaviour {

    public float speed; // TODO i should really do a base class an inheritance
    public Transform targetObject;
    [Range(0,360)]
    public float angleOffset;
    public bool updateDir;

    private Transform _transform;
    private Vector3 currentDir;
    private Vector3 intialDir;

    void Start () {
        _transform = GetComponent<Transform>();
    }
	
    void OnEnable() {
        currentDir = targetObject.position - transform.position;
        currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
    }

	void Update () {
        if (updateDir) {
            currentDir = targetObject.position - _transform.position;
            currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
        }

        _transform.position += currentDir.normalized * speed * Time.deltaTime;
    }

}
