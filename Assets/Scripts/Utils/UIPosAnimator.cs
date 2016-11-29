using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UIPosAnim {
    public string name;
    public float duration;
    public AnimationCurve animCurve;
}

public class UIPosAnimator : MonoBehaviour {

    [SerializeField] private List<UIPosAnim> animList = new List<UIPosAnim>();

    private RectTransform _rectTransform;
    private Dictionary<string, UIPosAnim> animations = new Dictionary<string, UIPosAnim>();

    void Start () {
        _rectTransform = GetComponent<RectTransform>();

        for(int i = 0; i < animList.Count; i++) {
            animations.Add(animList[i].name, animList[i]);
        }
    }

    public IEnumerator Animate(string name, Vector2 endPos) {
        float timer = 0;
        Vector2 startPos = _rectTransform.anchoredPosition;

        while (timer < animations[name].duration) {
            timer += Time.unscaledDeltaTime;

            float timePercent = timer / animations[name].duration;
            float animationCompletionPercent = animations[name].animCurve.Evaluate(timePercent);

            _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, animationCompletionPercent);

            yield return null;
        }

        _rectTransform.anchoredPosition = endPos;
    }
}
