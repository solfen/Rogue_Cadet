using UnityEngine;
using System.Collections;

public class LifeTime : MonoBehaviour, ISwitchable {

    public float lifeTime;
    private float timer = 0;
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= lifeTime) {
            Destroy(gameObject);
        }
	}

    public void SwitchState(bool state) {
        enabled = state;
    }
}
