using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, ISelectHandler {

    [SerializeField] private Image image;
    [SerializeField] private Text name;

    private int numberIncategory;
    private ShopDetailsUI shopDetailsPane;

    public void UpdateItem(ShopItem data, int _numberIncategory, ShopDetailsUI _shopDetailsPane) {
        image.sprite = data.sprite;
        name.text = data.title;

        numberIncategory = _numberIncategory;
        shopDetailsPane = _shopDetailsPane;
    }

    public void OnSelect (BaseEventData data) {
        shopDetailsPane.UpdateDetails(numberIncategory);
    }
}
