using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUIMain : MonoBehaviour {

    [SerializeField] private Image icon; 
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private GameObject unlocked;

    private string titleLocaID;
    private string descriptionLocaID;

    private int achievementIndex;

    void Start() {
        EventDispatcher.AddEventListener(Events.ACHIEVMENT_UNLOCKED, OnAchievementUnlocked);
        EventDispatcher.AddEventListener(Events.LOCALIZATION_CHANGED, OnLocaleChanged);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.ACHIEVMENT_UNLOCKED, OnAchievementUnlocked);
        EventDispatcher.RemoveEventListener(Events.LOCALIZATION_CHANGED, OnLocaleChanged);
    }

    public void Init(AchievementUI data, int _achievementIndex) {
        achievementIndex = _achievementIndex;
        icon.sprite = data.icon;
        titleLocaID = data.name;
        descriptionLocaID = data.description;
        unlocked.SetActive(GlobalData.instance.saveData.achievementsUnlocked.Contains(achievementIndex));

        UpdateUI();
    }

    private void UpdateUI() {
        if(titleLocaID != null && descriptionLocaID != null) { //init is not necessarily called at start
            title.text = LocalizationManager.GetLocalizedText(titleLocaID);
            description.text = LocalizationManager.GetLocalizedText(descriptionLocaID);
        }
    }

    private void OnAchievementUnlocked(object indexObj) {
        if((int)indexObj == achievementIndex) {
            unlocked.SetActive(true);
        }
    }

    private void OnLocaleChanged(object useless) {
        UpdateUI();
    }
}
