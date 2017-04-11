using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocaleUI : MonoBehaviour {

    public void OnPressed() {
        PlayerPrefs.SetInt("Language", PlayerPrefs.GetInt("Language", 0) == 0 ? 1 : 0);
        EventDispatcher.DispatchEvent(Events.LANGUAGE_PREF_CHANGED, null);
    }
}
