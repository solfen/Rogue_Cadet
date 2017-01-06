using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D other) {
        if(other.GetComponent<Bullet>() != null) {
            EventDispatcher.DispatchEvent(Events.SHIELD_ABSORB_BULLET, null);
        }
    }
}
