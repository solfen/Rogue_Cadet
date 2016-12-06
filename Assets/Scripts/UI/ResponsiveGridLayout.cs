using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResponsiveGridLayout : MonoBehaviour {

    public Transform target;

    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private Vector2 minPadding;
    [SerializeField] private Vector2 maxPadding;
    [SerializeField] private Vector2 minSpacing;
    [SerializeField] private Vector2 maxSpacing;
    [SerializeField] private bool isNavigable = true;

    public void Resize() {
        RectTransform rectTransform = target.GetComponent<RectTransform>();

        float containerArea = rectTransform.rect.width * rectTransform.rect.height;
        float maxItemSize = Mathf.Sqrt(containerArea / target.childCount); //sqrt "transforms" the area value into a square size value.
        float colNb = Mathf.Ceil(rectTransform.rect.width / maxItemSize);
        float rowNb = Mathf.Ceil(rectTransform.rect.height / maxItemSize);

        float containerRatio = (rectTransform.anchorMax.x - rectTransform.anchorMin.x) / (rectTransform.anchorMax.y - rectTransform.anchorMin.y);
        float sizeScaleRatio = (maxItemSize / rectTransform.rect.width - minSize) / (maxSize - minSize); // where we are on the item size scale
        Vector2 spacing = Vector2.Lerp(minSpacing, maxSpacing, sizeScaleRatio);
        Vector2 padding = Vector2.Lerp(minPadding, maxPadding, sizeScaleRatio);

        Vector2 totalSpacingSize = new Vector2((colNb - 1) * spacing.x, (rowNb - 1) * spacing.y);
        Vector2 itemSize = new Vector2((1 - padding.x * 2 - totalSpacingSize.x) / colNb, 0);
        itemSize.y = itemSize.x * Camera.main.aspect * containerRatio;

        for (int i = 0; i < target.childCount; i++) {
            Transform child = target.GetChild(i);
            float colID = i % colNb;
            float rowID = Mathf.Floor(i / colNb);
            RectTransform item = child.GetComponent<RectTransform>();
            item.anchorMin = new Vector2(padding.x + colID * (spacing.x + itemSize.x), 1 - (padding.y + rowID * (spacing.y + itemSize.y) + itemSize.y));
            item.anchorMax = new Vector2(item.anchorMin.x + itemSize.x, item.anchorMin.y + itemSize.y);

            if(isNavigable) {
                Navigation itemNav = new Navigation();
                itemNav.mode = Navigation.Mode.Explicit;
                itemNav.selectOnLeft = colID > 0 && i-1 >= 0 ? target.GetChild(i - 1).GetComponent<Button>() : null;
                itemNav.selectOnRight = colID < (colNb - 1) && i+1 < target.childCount ? target.GetChild(i + 1).GetComponent<Button>() : null;
                itemNav.selectOnDown = i + colNb < target.childCount ? target.GetChild((int)(i + colNb)).GetComponent<Button>() : null;
                itemNav.selectOnUp = i - colNb >= 0 ? target.GetChild((int)(i - colNb)).GetComponent<Button>() : null;
                child.GetComponent<Button>().navigation = itemNav;
            }
        }
    }
}
