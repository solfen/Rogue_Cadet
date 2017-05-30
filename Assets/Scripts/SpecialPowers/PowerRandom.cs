using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerRandom : BaseSpecialPower {

    [SerializeField] private List<BaseSpecialPower> powers;
    private BaseSpecialPower activePower = null;

    protected override void Start() {
        base.Start();
        activePower = Instantiate(powers[Random.Range(0, powers.Count)], transform.parent, false) as BaseSpecialPower;
    }

    protected override void Activate() {
        if(activePower.coolDownTimer <= 0)
            StartCoroutine(WaitForDestroy());
    }

    protected override void Update() {
        if(activePower.mana > 0) {
            base.Update();
        }
    }


    IEnumerator WaitForDestroy() {
        yield return null;
        while(activePower.coolDownTimer > 0) {
            yield return null;
        }

        float currentMana = activePower.mana;
        Debug.Log(currentMana);
        DestroyImmediate(activePower.gameObject);
        activePower = Instantiate(powers[Random.Range(0, powers.Count)], transform.parent, false) as BaseSpecialPower;
        yield return null;
        activePower.mana = currentMana;
        yield return null;
        EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_USED, activePower); //to activate the UI
    }
}
