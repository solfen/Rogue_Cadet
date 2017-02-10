using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShakeSlider : MonoBehaviour {

    [SerializeField] private Text volumeText;
    [SerializeField] private Slider slider;
	
    // Use this for initialization
	void Start () {
        slider.value = PlayerPrefs.GetFloat("ScreenShakeFore", 1);
        OnValueChange(slider.value);
    }
	
    public void OnValueChange(float value) {
        volumeText.text = "Screen Shake Force: " + Mathf.RoundToInt(value * 100) + "%";
        EventDispatcher.DispatchEvent(Events.SCREEN_SHAKE_MODIFIER_CHANGED, value);
        PlayerPrefs.SetFloat("ScreenShakeFore", value);
    }
}
