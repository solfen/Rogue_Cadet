using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class ResizeFontBasedOn1080pSize : MonoBehaviour {

    public float fullHDFontSize;
	// Use this for initialization
	void Awake() {
        Text text = GetComponent<Text>();
        text.fontSize = (int)(fullHDFontSize * Camera.main.pixelWidth / 1920);
    }

}
