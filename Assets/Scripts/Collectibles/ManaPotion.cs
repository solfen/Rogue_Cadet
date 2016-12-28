using UnityEngine;
using System.Collections;

public class ManaPotion : MonoBehaviour {
    public float manaToRegenerate;

    void OnTriggerEnter2D(Collider2D other) {
        BaseSpecialPower power = other.GetComponentInChildren<BaseSpecialPower>();
        if (power != null) {
            power.Regenerate(manaToRegenerate);
        }
    }

}
