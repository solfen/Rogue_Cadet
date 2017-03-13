using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour {

    [SerializeField] private Text manaText;
    [SerializeField] private Slider specialLoad;
    private BaseSpecialPower power;
    
    void Start () {
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_CREATED, OnPowerCreation);
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED, ManaChanged);
        EventDispatcher.AddEventListener(Events.MANA_POTION_TAKEN, ManaChanged);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_CREATED, OnPowerCreation);
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED, ManaChanged);
        EventDispatcher.RemoveEventListener(Events.MANA_POTION_TAKEN, ManaChanged);
    }

    private void OnPowerCreation(object powerObj) {
       power = (BaseSpecialPower)powerObj;
       ManaChanged(null);
    }

    private void ManaChanged(object useless) {
        manaText.text = (int)power.mana + "/" + power.maxMana;

        if(power.mana >= power.manaCost) {
            StartCoroutine(FillLoadBar());
        }
        else {
            specialLoad.value = 0;
        }
    }

    IEnumerator FillLoadBar() {
        specialLoad.value = 0;
        float timer = 0;
        float duration = power.coolDownTimer;

        while (timer < duration) {
            timer += Time.deltaTime;
            specialLoad.value = Mathf.Lerp(0, 1, timer/duration);
            yield return null;
        }

        specialLoad.value = 1;
        SoundManager.instance.PlaySound(GenericSoundsEnum.ACTIVATE);
    }
}
