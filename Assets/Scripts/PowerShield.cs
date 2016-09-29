using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShield : MonoBehaviour, ISpecialPower {

    public float duration;
    public GameObject shield;

    private CircleCollider2D shieldCollider;

    void Start() {
        shieldCollider = shield.GetComponent<CircleCollider2D>();
    }

    public void Activate() {
        StartCoroutine(ShieldTime());
    }

    IEnumerator ShieldTime() {
        float timer = duration;
        shield.SetActive(true);
        shieldCollider.enabled = true;

        while (timer > 0) {
            yield return null;
            timer -= Time.unscaledDeltaTime;
        }

        shield.SetActive(false);
        shieldCollider.enabled = false;

    }
}
