using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class LetterByLetter : MonoBehaviour {

    [SerializeField] private float letterInterval;
    private Text textUI;

	void Awake () {
        textUI = GetComponent<Text>();
    }

    void OnEnable () {
        StartCoroutine(LetterAnim());
    }

    IEnumerator LetterAnim() {
        string orignialText = textUI.text;
        textUI.text = "";

        for(int i = 0;  i < orignialText.Length; i++) {
            textUI.text += orignialText[i];
            
            for(float t = 0; t < letterInterval; t += Time.unscaledDeltaTime) {
                yield return null;
            }
        }
    }
}
