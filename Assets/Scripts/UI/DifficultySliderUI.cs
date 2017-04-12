using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MultiplierNames {
    PlayerLife,
    Gold,
    EnemiesLife,
    EnemiesBulletsDmg,
    EnemiesFireSpeed
}

// I hate this, it's a blatant duplicate from AudioSliderUI...
public class DifficultySliderUI : MonoBehaviour {

	[SerializeField] private MultiplierNames multiplierName;
    [SerializeField] private Text sliderText;
    [SerializeField] private Slider slider;

    private string baseText;

    void Start() {
        baseText = sliderText.text;

        float baseValue = PlayerPrefs.GetFloat(multiplierName + "Multiplier", 1);
        slider.value = baseValue;
        OnValueChange(baseValue);

        EventDispatcher.AddEventListener(Events.PLAYER_SKILLS_SNIFFED, OnSniffed);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_SKILLS_SNIFFED, OnSniffed);
    }

    public void OnValueChange(float value) {
        sliderText.text = baseText + ": x" + value.ToString("F2");
        PlayerPrefs.SetFloat(multiplierName + "Multiplier", value);
        EventDispatcher.DispatchEvent(Events.DIFFICULTY_CHANGED, null);
    }

    public void OnSniffed(object useless) {
        float baseValue = PlayerPrefs.GetFloat(multiplierName + "Multiplier", 1);
        slider.value = baseValue;
        sliderText.text = baseText + ": x" + baseValue.ToString("F2");
    }
}
