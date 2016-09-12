using UnityEngine;
using System.Collections;

public class TargetMovement : BaseMovement {

    public Transform targetObject;
    [Range(0,360)]
    public float angleOffset;
    public bool updateDir;

    private Vector3 currentDir;
    private Vector3 intialDir;
	
    void OnEnable() {
        currentDir = targetObject.position - transform.position;
        currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
    }

	void Update () {
        if(targetObject == null) {
            return;
        }

        if (updateDir) {
            currentDir = targetObject.position - _transform.position;
            currentDir = Quaternion.Euler(0, 0, angleOffset) * currentDir;
        }

        _transform.position += currentDir.normalized * speed * Time.deltaTime;
    }

}
