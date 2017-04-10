﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {

    [SerializeField] private LocalizationDataBase db;
    private static int currentLanguage = 0;
    private static Dictionary<string, LocalizedText> texts = new Dictionary<string, LocalizedText>();

    void Awake() {
        for(int i = 0; i < db.texts.Count; i++) {
            texts.Add(db.texts[i].id, db.texts[i]);
        }

        currentLanguage = PlayerPrefs.GetInt("Language", 1);

        EventDispatcher.AddEventListener(Events.LANGUAGE_PREF_CHANGED, OnLangChanged);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.LANGUAGE_PREF_CHANGED, OnLangChanged);
    }

    private void OnLangChanged(object useless) {
        currentLanguage = PlayerPrefs.GetInt("Language", 1);
        EventDispatcher.DispatchEvent(Events.LOCALIZATION_CHANGED, null);
    }

    public static string GetLocalizedText(string textID) {
        if(!texts.ContainsKey(textID)) {
            Debug.LogError("LocalizedText Key: " + textID + " doesn't exist");
            return "";
        }

        return texts[textID].languagesTexts[currentLanguage];
    }
}
