using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipDetailsPane : MonoBehaviour {

    [SerializeField] private GameData gameData;
    [SerializeField] private ShipTypesUI shipTypes;
    [SerializeField] private ShipSelector shipSelector;

    [Header("UI child elems")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text description;
    [SerializeField] private GameObject statsParent;
    [SerializeField] private Text life;
    [SerializeField] private Text dps;
    [SerializeField] private Text mana;
    [SerializeField] private GameObject achievementParent;
    [SerializeField] private Image achivementImage;
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
        int shipIndex = gameData.shipsUIItems[selectedShip].types[selectedType].associatedShipIndex;

        if (GlobalData.instance.saveData.shipsInfo[shipIndex].isUnlocked) {
            achievementParent.SetActive(false);
            statsParent.SetActive(true);
            nameText.text = gameData.shipsUIItems[selectedShip].name;
            description.text = gameData.shipsUIItems[selectedShip].types[selectedType].description;
            powerName.text = gameData.shipsUIItems[selectedShip].types[selectedType].powerName;
            powerDescription.text = gameData.shipsUIItems[selectedShip].types[selectedType].powerDescription;

            ShipConfig shipConfig = gameData.ships[shipIndex];
            life.text = "- Life: " + (int)(shipConfig.lifePrecent*100) + "%";
            dps.text = "- Damage: " + (int)(shipConfig.damagePrecent * 100) + "%";
            mana.text = "- Mana: " + (int)(shipConfig.manaPrecent * 100) + "%";
        }
        else {
            AchievementUI achiev = gameData.achievementsUI[gameData.shipsUIItems[selectedShip].types[selectedType].associatedAchievementIndex];
            achievementParent.SetActive(true);
            statsParent.SetActive(false);
            nameText.text = "Locked";
            description.text = "Achievement required";
            achivementImage.sprite = achiev.icon;
            powerName.text = achiev.name;
            powerDescription.text = achiev.description;
        }
    }
}
