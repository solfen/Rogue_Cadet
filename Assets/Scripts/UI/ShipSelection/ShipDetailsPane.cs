using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipDetailsPane : MonoBehaviour {

    //GameData ref
    //UI elements refs
    [SerializeField] private GameData gameData;
    [SerializeField] private ShipTypesUI shipTypes;
    [SerializeField] private ShipSelector shipSelector;

    [Header("UI child elems")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text description;
    [SerializeField] private Text life;
    [SerializeField] private Text dps;
    [SerializeField] private Text mana;
    [SerializeField] private Text powerName;
    [SerializeField] private Text powerDescription;

    private RectTransform _rectTransform;
    private UIPosAnimator animator;
    private bool isOpen = false;

    private int selectedShip;
    private int selectedType;

    void Start() {
        _rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<UIPosAnimator>();
    }

    void Update() {
        if(!isOpen) {
            return;
        }

        if (Input.GetButtonDown("Cancel")) {
            StartCoroutine(Close());
            isOpen = false;
        }
        else if(Input.GetButtonDown("Submit")) {
            shipSelector.SelectShip(gameData.shipsUIItems[selectedShip].types[selectedType].associatedShipIndex);
        }
    }

    public void Open() {
        StartCoroutine(animator.Animate("toggleVisibility", new Vector2(0, _rectTransform.anchoredPosition.y)));
        isOpen = true;
    }

    IEnumerator Close() {
        yield return StartCoroutine(animator.Animate("toggleVisibility", new Vector2((_rectTransform.anchorMax.x - _rectTransform.anchorMin.x) * 2 * Camera.main.pixelWidth, _rectTransform.anchoredPosition.y)));
        shipTypes.Close();
    }

    public void UpdateDetails(int _selectedShip, int _selectedType) {
        selectedShip = _selectedShip;
        selectedType = _selectedType;

        //TODO: check if isUnlocked and then change the data

        nameText.text = gameData.shipsUIItems[selectedShip].name;
        description.text = gameData.shipsUIItems[selectedShip].types[selectedType].description;
        powerName.text = gameData.shipsUIItems[selectedShip].types[selectedType].powerName;
        powerDescription.text = gameData.shipsUIItems[selectedShip].types[selectedType].powerDescription;

        ShipConfig shipConfig = gameData.ships[gameData.shipsUIItems[selectedShip].types[selectedType].associatedShipIndex];
        life.text = "- Life: " + shipConfig.lifePrecent + "%";
        dps.text = "- Damage: " +  shipConfig.damagePrecent + "%";
        life.text = "- Mana: " + shipConfig.manaPrecent + "%";
    }
}
