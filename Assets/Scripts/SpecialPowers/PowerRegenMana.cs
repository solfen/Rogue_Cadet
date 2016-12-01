using UnityEngine;
using System.Collections;
using System;

public class PowerRegenMana : BaseSpecialPower {

    [SerializeField] private float duration;
    [SerializeField] private float amountRegen;

    protected override void Activate() {
        StartCoroutine(Regen());
    }

    IEnumerator Regen() {
        yield return null;
        float timer = 0;
        float startMana = mana;

        while(timer < duration) {
            mana = Mathf.Min((int)(Mathf.Lerp(startMana, startMana + amountRegen, timer / duration)), maxMana);

            timer += Time.deltaTime;
            yield return null;
        }

        Mathf.Min(startMana + amountRegen, maxMana);
    }
}
