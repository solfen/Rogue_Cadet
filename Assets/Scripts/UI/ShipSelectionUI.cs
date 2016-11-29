using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSelectionUI : MonoBehaviour {

    public GameObject firstUpgradeItem;
    public EventSystem eventSystem;
    public ShopUI shopUI;
    public GameObject shipUgradeUI;

    [SerializeField] private Animator transitionAnimator;

    public void ValidateSelection(int selectedShip) {
        shopUI.enabled = true;
        shipUgradeUI.SetActive(true);
        PlayerPrefs.SetInt("SelectedShip", selectedShip);
        transitionAnimator.SetTrigger("Transition");
        eventSystem.SetSelectedGameObject(firstUpgradeItem);
    }
}
