using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShield : BaseSpecialPower {

    [SerializeField] private float duration;
    [SerializeField] private GameObject shield;

    public override void Activate() {
        StartCoroutine(ShieldTime());
    }

    IEnumerator ShieldTime() {
        float timer = duration;
        shield.SetActive(true);

        while (timer > 0) {
            yield return null;
            timer -= Time.unscaledDeltaTime;
        }

        shield.SetActive(false);
    }
}
