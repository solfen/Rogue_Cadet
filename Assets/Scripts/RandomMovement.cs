using UnityEngine;
using System.Collections;

public class RandomMovement : BaseMovement {

    [Range(0,360)]
    public float minRandAngle;
    [Range(0, 360)]
    public float maxRandAngle;
    public float changeAngleInterval;

    private Vector3 currentDir;
    private float angle;
    private float timer = 0;
	
	void Update() {
        if(timer <= 0) {
            angle = Random.Range(minRandAngle, maxRandAngle) * Mathf.Deg2Rad;
            currentDir.Set(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            timer = changeAngleInterval;
        }

        _transform.position += currentDir * speed * Time.deltaTime;

        timer -= Time.deltaTime;
    }

}
