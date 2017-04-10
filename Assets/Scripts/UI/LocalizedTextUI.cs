using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedTextUI : MonoBehaviour {

    [SerializeField] private string textID; 
    private Text _text;

	void Start () {
        _text = GetComponent<Text>();

        LocalizeText();

        EventDispatcher.AddEventListener(Events.LOCALIZATION_CHANGED, LocalizeText);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.LOCALIZATION_CHANGED, LocalizeText);
    }

    private void LocalizeText(object useless = null) {
        _text.text = LocalizationManager.GetLocalizedText(textID);
    }
}
