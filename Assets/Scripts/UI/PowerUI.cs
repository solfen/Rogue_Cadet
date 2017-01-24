using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour {

    [SerializeField] private Text manaText;
    [SerializeField] private Slider specialLoad;
    
    void Start () {
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED, OnUsePower);
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_CREATED, OnUsePower);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED, OnUsePower);
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_CREATED, OnUsePower);
    }

    public void OnUsePower(object powerObj) {
        BaseSpecialPower power = (BaseSpecialPower)powerObj;
        manaText.text = (int)power.mana + "/" + power.maxMana;

        StartCoroutine(FillLoadBar(power));
    }

    IEnumerator FillLoadBar(BaseSpecialPower power) {
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
