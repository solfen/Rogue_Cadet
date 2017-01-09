using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementUIMain : MonoBehaviour {

    [SerializeField] private Image icon; 
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private GameObject unlocked;

    public void Init(AchievementUI data, bool isUnlocked) {
        icon.sprite = data.icon;
        title.text = data.name;
        description.text = data.description;
        unlocked.SetActive(isUnlocked);
    }
}
