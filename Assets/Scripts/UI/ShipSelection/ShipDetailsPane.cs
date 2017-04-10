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
        EventDispatcher.DispatchEvent(Events.CLOSE_UI_PANE, null);
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
            nameText.text = LocalizationManager.GetLocalizedText(gameData.shipsUIItems[selectedShip].types[selectedType].name);
            description.text = LocalizationManager.GetLocalizedText(gameData.shipsUIItems[selectedShip].types[selectedType].description);
            powerName.text = LocalizationManager.GetLocalizedText(gameData.shipsUIItems[selectedShip].types[selectedType].powerName);
            powerDescription.text = LocalizationManager.GetLocalizedText(gameData.shipsUIItems[selectedShip].types[selectedType].powerDescription);

            ShipConfig shipConfig = gameData.ships[shipIndex];
            life.text = "- " + LocalizationManager.GetLocalizedText("SHIPS_DETAILS_LIFE") + (int)(shipConfig.lifePrecent*100) + "%";
            dps.text = "- " + LocalizationManager.GetLocalizedText("SHIPS_DETAILS_DAMAGE") + (int)(shipConfig.damagePrecent * 100) + "%";
            mana.text = "- " + LocalizationManager.GetLocalizedText("SHIPS_DETAILS_MANA") + (int)(shipConfig.manaPrecent * 100) + "%";
        }
        else {
            AchievementUI achiev = gameData.achievementsUI[gameData.shipsUIItems[selectedShip].types[selectedType].associatedAchievementIndex];
            achievementParent.SetActive(true);
            statsParent.SetActive(false);
            nameText.text = LocalizationManager.GetLocalizedText("SHIPS_DETAILS_LOCKED");
            description.text = gameData.shipsUIItems[selectedShip].types[selectedType].associatedAchievementIndex  < 12 ? LocalizationManager.GetLocalizedText("SHIPS_DETAILS_ACHIEVEMENT_REQUIRED") : LocalizationManager.GetLocalizedText("SHIPS_DETAILS_UPGRADE_REQUIRED");
            achivementImage.sprite = achiev.icon;
            powerName.text = LocalizationManager.GetLocalizedText(achiev.name);
            powerDescription.text = LocalizationManager.GetLocalizedText(achiev.description);
        }
    }
}
