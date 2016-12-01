using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour {

    [SerializeField] private Text manaText;
    [SerializeField] private Slider specialLoad;
    
    void Start () {
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED, OnUsePower);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED, OnUsePower);
    }

    public void OnUsePower(object powerObj) {
        BaseSpecialPower power = (BaseSpecialPower)powerObj;

        manaText.text = power.mana + "/" + power.maxMana;
        StartCoroutine(FillLoadBar(power.coolDownTimer));
    }

    IEnumerator FillLoadBar(float duration) {
        specialLoad.value = 0;
        float timer = 0;

        while(timer < duration) {
            timer += Time.deltaTime;
            specialLoad.value = Mathf.Lerp(0, 1, timer/duration);
            yield return null;
        }

        specialLoad.value = 1;
        SoundManager.instance.PlaySound(GenericSoundsEnum.ACTIVATE);
    }
}
