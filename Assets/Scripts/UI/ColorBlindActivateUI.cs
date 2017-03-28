using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlindActivateUI : MonoBehaviour {

    [SerializeField] private Text stateText;

	// Use this for initialization
	void Start () {
        SetTextFeedback();
    }
	
    public void OnPressed() {
        PlayerPrefs.SetInt("ColorBlindMode", PlayerPrefs.GetInt("ColorBlindMode", 0) == 0 ? 1 : 0);
        EventDispatcher.DispatchEvent(Events.COLORBLIND_MODE_CHANGED, null);
        SetTextFeedback();
    }

    private void SetTextFeedback() {
        stateText.text = PlayerPrefs.GetInt("ColorBlindMode", 0) == 0 ? "(Off)" : "(On)";
    }
}
