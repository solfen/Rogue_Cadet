using UnityEngine;
using System.Collections;

public class ActivateBlink : MonoBehaviour {

    public GameObject target;
    public float activeDuration;
    public float inactiveDuration;

    private float timer = 0;

    void Start() {
        timer = target.activeSelf ? activeDuration : inactiveDuration;
    }
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

	    if(timer < 0) {
            target.SetActive(!target.activeSelf);
            timer = target.activeSelf ? activeDuration : inactiveDuration;
        }
	}
}
