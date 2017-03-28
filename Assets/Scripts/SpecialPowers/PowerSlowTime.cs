using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSlowTime : BaseSpecialPower {

    [SerializeField] private float duration;
    [SerializeField] private float timeFactor;

    protected override void Activate() {
        StartCoroutine(SlowTime());
    }

    IEnumerator SlowTime() {
        float timer = duration;
        Time.timeScale = timeFactor;

        while (timer > 0) {
            yield return null;
            timer -= Time.deltaTime / timeFactor;
        }

        Time.timeScale = 1;
    }
}
