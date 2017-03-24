using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlindMode : MonoBehaviour {

    [SerializeField] private Color colorBlindColor;

	// Use this for initialization
	void Awake () {
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        Graphic UIRender = GetComponent<Graphic>();

        if (spriteRender == null && UIRender == null)
            return;

        if (PlayerPrefs.GetInt("ColorBlindMode", 0) == 1) {
            if(spriteRender != null)
                spriteRender.color = colorBlindColor;
            else
                UIRender.color = colorBlindColor;
        }
    }
	
}
