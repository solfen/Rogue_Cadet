using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipUI : MonoBehaviour {

    [SerializeField] private Image shipImage;
    [SerializeField] private Text shipName;
	
    public void Init (ShipsUIItemData data, int elemIndex, Vector2 halfsize) {
        RectTransform rectTrans = GetComponent<RectTransform>();
        rectTrans.anchorMin = new Vector2(elemIndex * 0.5f - halfsize.x, 0.5f - halfsize.y * Camera.main.aspect);
        rectTrans.anchorMax = new Vector2(elemIndex * 0.5f + halfsize.x, 0.5f + halfsize.y * Camera.main.aspect);
        rectTrans.offsetMax = Vector2.zero;
        rectTrans.offsetMin = Vector2.zero;

        shipImage.sprite = data.spriteUI;
        shipName.text = data.name;
    }
}
