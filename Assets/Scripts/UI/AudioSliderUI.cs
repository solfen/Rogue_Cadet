using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum AudioMixerNames {
    Master,
    Music,
    Sounds
}

public class AudioSliderUI : MonoBehaviour {

    [SerializeField] private AudioMixerNames mixerName;
    [SerializeField] private string localizedTextID;
    [SerializeField] private Text volumeText;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;

    void Start() {
        float baseValue = PlayerPrefs.GetFloat(mixerName + "Volume", 0);
        slider.value = baseValue;
        OnValueChange(baseValue);
    }
	
    public void OnValueChange(float value) {
        volumeText.text = LocalizationManager.GetLocalizedText(localizedTextID) + (value >= 0 ? "+" : "") + value + " dB";
        mixer.SetFloat(mixerName + "Volume", value);
        PlayerPrefs.SetFloat(mixerName + "Volume", value);
    }
}
