using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneText : MonoBehaviour, IInteractable {

    [SerializeField] private Text text;
    [SerializeField] private LetterByLetter textLetterAnim;
    [SerializeField] private CutSceneText nextText;
    [SerializeField] private GameObject elemActivateOnEnd;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float intervalBetweenTexts = 0.5f;
    [SerializeField] private bool autoStart = false;

    private bool isFadingOut = false;
    private bool isFadingIn = false;

    void Start() {
        if(autoStart) {
            Activate();
        }
    }

    void Update() {
        if(Input.GetButtonDown("Submit") && !isFadingOut) {
            StopAllCoroutines();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

            if(isFadingIn) {
                isFadingIn = false;
                return;
            }

            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeText(float startAlpha, float endAlpha) {
        Color baseColor = new Color(text.color.r, text.color.g, text.color.b, startAlpha);
        Color endColor = new Color(baseColor.r, baseColor.g, baseColor.b, endAlpha);
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime) {
            text.color = Color.Lerp(baseColor, endColor, t / fadeDuration);
            yield return null;
        }

        text.color = endColor;
    }

    IEnumerator FadeGraphicAlpha(float startAlpha, float endAlpha) {
        text.CrossFadeAlpha(startAlpha, 0, true);
        text.CrossFadeAlpha(endAlpha, fadeDuration, true);
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime) {
            yield return null;
        }
    } 

    IEnumerator FadeOut() {
        isFadingOut = true;
        yield return StartCoroutine(FadeText(1, 0));
        yield return StartCoroutine(FadeGraphicAlpha(1, 0));
        isFadingOut = false;

        gameObject.SetActive(false);

        if(nextText != null)
            nextText.Activate();
        if (elemActivateOnEnd != null)
            elemActivateOnEnd.GetComponent<IInteractable>().Activate();

    }

    IEnumerator FadeIn() {
        isFadingIn = true;
        yield return StartCoroutine(FadeGraphicAlpha(0, 1));
        yield return StartCoroutine(FadeText(0, 1));
        isFadingIn = false;
    }

    public void Activate() {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }
}
