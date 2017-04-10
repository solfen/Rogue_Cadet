using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUIMain : MonoBehaviour {

    [SerializeField] private Image icon; 
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private GameObject unlocked;

    private int achievementIndex;

    void Start() {
        EventDispatcher.AddEventListener(Events.ACHIEVMENT_UNLOCKED, OnAchievementUnlocked);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.ACHIEVMENT_UNLOCKED, OnAchievementUnlocked);
    }

    public void Init(AchievementUI data, int _achievementIndex) {
        achievementIndex = _achievementIndex;
        icon.sprite = data.icon;
        title.text = LocalizationManager.GetLocalizedText(data.name);
        description.text = LocalizationManager.GetLocalizedText(data.description);
        unlocked.SetActive(GlobalData.instance.saveData.achievementsUnlocked.Contains(achievementIndex));
    }

    private void OnAchievementUnlocked(object indexObj) {
        if((int)indexObj == achievementIndex) {
            unlocked.SetActive(true);
        }
    }
}
