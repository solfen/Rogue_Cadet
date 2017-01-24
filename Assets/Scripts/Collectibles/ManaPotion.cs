using UnityEngine;
using System.Collections;

public class ManaPotion : MonoBehaviour {
    public float manaToRegenerate;

    void OnTriggerEnter2D(Collider2D other) {
        EventDispatcher.DispatchEvent(Events.MANA_POTION_TAKEN, this);
    }

}
