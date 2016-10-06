using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSlowTime : MonoBehaviour, ISpecialPower {

    public float duration;
    public float timeFactor;

    public void Activate() {
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
