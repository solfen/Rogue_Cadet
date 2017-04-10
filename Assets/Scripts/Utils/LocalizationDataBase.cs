using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LocalizationDataBase : ScriptableObject {
    public List<LocalizedText> texts;
}

[System.Serializable]
public class LocalizedText {
    public string id;
    public List<string> languagesTexts;
}
