using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSelectionUI : MonoBehaviour {

    public GameObject firstUpgradeItem;
    public EventSystem eventSystem;
    public GameObject shipUgradeUI;

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private ShipSelector shipSelector;

    public void ValidateSelection(int selectedShip) {
        shipUgradeUI.SetActive(true);
        transitionAnimator.SetTrigger("Transition");
        eventSystem.SetSelectedGameObject(firstUpgradeItem);

        shipSelector.SelectShip(selectedShip);
    }
}
