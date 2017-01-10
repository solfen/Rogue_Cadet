using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerShield : BaseSpecialPower {

    [SerializeField] private float duration;
    [SerializeField] private GameObject shield;
    private Player player;

    protected override void Start() {
        base.Start();
        player = transform.parent.GetComponent<Player>();
    }

    protected override void Activate() {
        StartCoroutine(ShieldTime());
    }

    IEnumerator ShieldTime() {
        float timer = duration;
        shield.SetActive(true);
        player.invincibiltyTimer = duration;

        while (timer > 0) {
            yield return null;
            timer -= Time.unscaledDeltaTime;
        }

        shield.SetActive(false);
    }
}
