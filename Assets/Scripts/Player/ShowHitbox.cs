using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class ShowHitbox : MonoBehaviour {

    public BoxCollider2D realHitbox;
    public float blinkInterval;

    private LineRenderer line;
    private Vector3[] points = new Vector3[5];
    private bool activated = false;

	void Start () {
        line = GetComponent<LineRenderer>();

        float middleInHitboxX = realHitbox.transform.position.x + realHitbox.offset.x - transform.position.x;
        float middleInHitboxY = realHitbox.transform.position.y + realHitbox.offset.y - transform.position.y;
        points[0] = new Vector3(middleInHitboxX - realHitbox.size.x / 2, middleInHitboxY + realHitbox.size.y / 2,0);
        points[1] = new Vector3(middleInHitboxX + realHitbox.size.x / 2, middleInHitboxY + realHitbox.size.y / 2,0);
        points[2] = new Vector3(middleInHitboxX + realHitbox.size.x / 2, middleInHitboxY - realHitbox.size.y / 2,0);
        points[3] = new Vector3(middleInHitboxX - realHitbox.size.x / 2, middleInHitboxY - realHitbox.size.y / 2,0);
        points[4] = points[0];

        line.SetPositions(points);
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.timeScale != 0 && InputManager.GetButtonDown(InputManager.GameButtonID.SHOW_HITBOX)) {
            activated = !activated;
            line.enabled = !line.enabled;

            /*if(activated) {
                StartCoroutine(Blink());
            }*/
        }

	}

    IEnumerator Blink() {
        float timer = 0f;

        while(activated) {
            if(timer <= 0) {
                line.enabled = !line.enabled;
                timer = blinkInterval;
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        line.enabled = false;
    }
}
