using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShipTypeItem {
    public Image image;
    public Text stock;
}

public class ShipTypesUI : MonoBehaviour {

    [SerializeField] private ShipListUI shipList;
    [SerializeField] private ShipDetailsPane detailsPane;
    [SerializeField] private GameObject firstSlectedObject;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<ShipTypeItem> shipTypesUI;
    [SerializeField] private Sprite lockedSprite;

    private GameData gameData;
    private RectTransform _rectTransform;
    private UIPosAnimator animator;
    private RectTransform detailsTrans;
    private int selectedShip;
    private int selectedLayout;

    // Use this for initialization
    void Start () {
        _rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<UIPosAnimator>();
        detailsTrans = detailsPane.GetComponent<RectTransform>();
        gameData = GlobalData.instance.gameData;
    }

    public void PopDown(int _selectedShip) {
        selectedShip = _selectedShip;

        for (int i = 0; i < shipTypesUI.Count; i++) {
            ShipTypeUIItem data = gameData.shipsUIItems[selectedShip].types[i];
            ShipInfo savedData = GlobalData.instance.saveData.shipsInfo[data.associatedShipIndex];
            int currentStock = (int)(savedData.stock);

            if(savedData.isUnlocked) {
                shipTypesUI[i].image.sprite = data.typeSprite;
                shipTypesUI[i].image.color = currentStock >= 1 ? Color.white : Color.grey;
                shipTypesUI[i].stock.text = "Stock: " + currentStock + "/" + gameData.ships[data.associatedShipIndex].maxStock;
            }
            else {
                shipTypesUI[i].image.sprite = lockedSprite;
                shipTypesUI[i].stock.text = "Stock: ?";
            }
        }

        StartCoroutine("PopDownAnim");
    }

    IEnumerator PopDownAnim() {
        yield return StartCoroutine(animator.Animate("toggleVisibility", new Vector2(_rectTransform.anchoredPosition.x, 0)));
        yield return new WaitForSeconds(0.25f);
        detailsPane.Open();
        StartCoroutine(animator.Animate("shifting", new Vector2(-(detailsTrans.anchorMax.x - detailsTrans.anchorMin.x) * 0.5f * Camera.main.pixelWidth, 0)));
        eventSystem.SetSelectedGameObject(firstSlectedObject);
    }

    public void SelectShipType(int layoutID) {
        selectedLayout = layoutID;
        detailsPane.UpdateDetails(selectedShip, selectedLayout);
    }

    public void Close() {
        StartCoroutine(CloseAnim());
    }

    IEnumerator CloseAnim() {
        yield return StartCoroutine(animator.Animate("shifting", Vector2.zero));
        yield return StartCoroutine(animator.Animate("toggleVisibility", new Vector2(0, Camera.main.pixelHeight)));
        shipList.Open();
    }
}
