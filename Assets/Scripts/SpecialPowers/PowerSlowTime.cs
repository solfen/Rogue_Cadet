using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSlowTime : BaseSpecialPower {

    [SerializeField] private float duration;
    [SerializeField] private float timeFactor;

    public override void Activate() {
        StartCoroutine(SlowTime());
    }

    IEnumerator SlowTime() {
        float timer = duration;
        Time.timeScale = timeFactor;

        while (timer > 0) {
            yield return null;
            timer -= Time.unscaledDeltaTime;
        }

        Time.timeScale = 1;
    }
}
