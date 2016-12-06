using UnityEngine;
using System.Collections;

public class ResponsiveGridLayout : MonoBehaviour {

    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private Vector2 minPadding;
    [SerializeField] private Vector2 maxPadding;
    [SerializeField] private Vector2 minSpacing;
    [SerializeField] private Vector2 maxSpacing;

    private Transform _transform;
    private RectTransform rectTransform;

    // Use this for initialization
    void Start () {
        _transform = GetComponent<Transform>();
        rectTransform = GetComponent<RectTransform>();
        Resize();
    }

    public void Resize() {
        float containerArea = rectTransform.rect.width * rectTransform.rect.height;
        float maxItemSize = Mathf.Sqrt(containerArea / _transform.childCount); //sqrt "transforms" the area value into a square size value.
        float colNb = Mathf.Ceil(rectTransform.rect.width / maxItemSize);
        float rowNb = Mathf.Ceil(rectTransform.rect.height / maxItemSize);

        float containerRatio = (rectTransform.anchorMax.x - rectTransform.anchorMin.x) / (rectTransform.anchorMax.y - rectTransform.anchorMin.y);
        float sizeScaleRatio = (maxItemSize / rectTransform.rect.width - minSize) / (maxSize - minSize); // where we are on the item size scale
        Vector2 spacing = Vector2.Lerp(minSpacing, maxSpacing, sizeScaleRatio);
        Vector2 padding = Vector2.Lerp(minPadding, maxPadding, sizeScaleRatio);

        Vector2 totalSpacingSize = new Vector2((colNb - 1) * spacing.x, (rowNb - 1) * spacing.y);
        Vector2 itemSize = new Vector2((1 - padding.x * 2 - totalSpacingSize.x) / colNb, 0);
        itemSize.y = itemSize.x * Camera.main.aspect * containerRatio;

        for (int i = 0; i < _transform.childCount; i++) {
            float colID = i % colNb;
            float rowID = Mathf.Floor(i / colNb);
            RectTransform item = _transform.GetChild(i).GetComponent<RectTransform>();
            item.anchorMin = new Vector2(padding.x + colID * (spacing.x + itemSize.x), 1 - (padding.y + rowID * (spacing.y + itemSize.y) + itemSize.y));
            item.anchorMax = new Vector2(item.anchorMin.x + itemSize.x, item.anchorMin.y + itemSize.y);
        }
    }
}
