using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUI : MonoBehaviour {

    public static PowerUI instance;

    [SerializeField]
    private Text manaText;
    [SerializeField]
    private Slider specialLoad;

	// Use this for initialization
	void Awake () {
        instance = this;
    }
	
    public void OnUsePower(SpecialPower power) {
        manaText.text = "Special mana: " + power.mana + "/" + power.maxMana;
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
