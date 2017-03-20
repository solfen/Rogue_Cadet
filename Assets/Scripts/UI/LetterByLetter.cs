using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class LetterByLetter : MonoBehaviour {

    public bool isWriting; 
    [SerializeField] private float letterInterval;
    private Text textUI;
    private string orignialText;

    void Awake () {
        textUI = GetComponent<Text>();
    }

    void OnEnable () {
        isWriting = true;
        StartCoroutine(LetterAnim());
    }

    IEnumerator LetterAnim() {
        orignialText = textUI.text;
        textUI.text = "";

        for(int i = 0;  i < orignialText.Length; i++) {
            textUI.text += orignialText[i];
            
            for(float t = 0; t < letterInterval; t += Time.unscaledDeltaTime) {
                yield return null;
            }
        }

        isWriting = false;
    }

    public void FinishEarly() {
        StopAllCoroutines();
        textUI.text = orignialText;
        isWriting = false;
    }
}
