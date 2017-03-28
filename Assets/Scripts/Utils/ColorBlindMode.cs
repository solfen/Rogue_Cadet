using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlindMode : MonoBehaviour {

    [SerializeField] private Color colorBlindColor;
    private Color baseColor;

    private bool hasColor;
    private SpriteRenderer spriteRender;
    private Graphic UIRender;

    // Use this for initialization
    void Awake () {
        spriteRender = GetComponent<SpriteRenderer>();
        UIRender = GetComponent<Graphic>();

        hasColor = spriteRender != null || UIRender != null;
        if (!hasColor) {
            Debug.LogWarning("ColorBlindMode on non-color object. name: " + gameObject.name, this);
            return;
        }

        baseColor = spriteRender != null ? spriteRender.color : UIRender.color;

        CheckColorMode();
        EventDispatcher.AddEventListener(Events.COLORBLIND_MODE_CHANGED, OnColorBlindModeChange);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.COLORBLIND_MODE_CHANGED, OnColorBlindModeChange);
    }

    private void CheckColorMode() {
        if (hasColor) {
            Color newColor = PlayerPrefs.GetInt("ColorBlindMode", 0) == 1 ? colorBlindColor : baseColor;

            if (spriteRender != null)
                spriteRender.color = newColor;
            else
                UIRender.color = newColor;
        }
    }

    private void OnColorBlindModeChange(object useless) {
        CheckColorMode();
    }
}
