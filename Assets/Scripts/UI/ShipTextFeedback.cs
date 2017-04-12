using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable] 
public class TextFeedbackColors {
    public Color hit;
    public Color manaPotion;
    public Color healthPotion;
    public Color goldCoin;
    public Color bomb;
}

public class ShipTextFeedback : MonoBehaviour {

    [SerializeField] private Text feedback;
    [SerializeField] private Animator anim;
    [SerializeField] private TextFeedbackColors normalColors;
    [SerializeField] private TextFeedbackColors colorBlindColors;

    private Transform _transform;
    private float lastPlayerLife = -1;
    private TextFeedbackColors feedbackColors;
    // Use this for initialization
    void Start () {
        _transform = GetComponent<Transform>();
        EventDispatcher.AddEventListener(Events.PLAYER_HIT, OnPlayerHit);
        EventDispatcher.AddEventListener(Events.MANA_POTION_TAKEN, OnManaPotionTaken);
        EventDispatcher.AddEventListener(Events.HEALTH_POTION_TAKEN, OnHealthPotionTaken);
        EventDispatcher.AddEventListener(Events.COLLECTIBLE_TAKEN, OnGoldCoinTaken);
        EventDispatcher.AddEventListener(Events.BOMB_COLLECTIBLE_TAKEN, OnBombTaken);

        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.AddEventListener(Events.COLORBLIND_MODE_CHANGED, OnColorBlindModeChange);

        OnColorBlindModeChange(null);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_HIT, OnPlayerHit);
        EventDispatcher.RemoveEventListener(Events.MANA_POTION_TAKEN, OnManaPotionTaken);
        EventDispatcher.RemoveEventListener(Events.HEALTH_POTION_TAKEN, OnHealthPotionTaken);
        EventDispatcher.RemoveEventListener(Events.COLLECTIBLE_TAKEN, OnGoldCoinTaken);
        EventDispatcher.RemoveEventListener(Events.BOMB_COLLECTIBLE_TAKEN, OnBombTaken);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        EventDispatcher.RemoveEventListener(Events.COLORBLIND_MODE_CHANGED, OnColorBlindModeChange);
    }

    private void OnColorBlindModeChange(object useless) {
        feedbackColors = PlayerPrefs.GetInt("ColorBlindMode", 0) == 1 ? colorBlindColors : normalColors;
    }

    void Update() {
        _transform.up = Vector3.up;
    }

    private void OnPlayerHit(object playerObj) {
        Player player = (Player)playerObj;
        if(player.currentLife <= 0) {
            return;
        }

        lastPlayerLife = lastPlayerLife == -1 ? player.maxLife : Mathf.Min(lastPlayerLife, player.maxLife); // need to do a max() because of dumb += in HealthPotionTaken

        feedback.color = feedbackColors.hit;
        feedback.text = "-" + ((int)(lastPlayerLife - player.currentLife)) + " " + LocalizationManager.GetLocalizedText("SHIP_TEXT_FEEDBACK_HP");
        anim.SetTrigger("Open");

        lastPlayerLife = player.currentLife;
    }

    private void OnManaPotionTaken(object manaPotion) {
        feedback.color = feedbackColors.manaPotion;
        feedback.text = "+" + ((int)((ManaPotion)manaPotion).manaToRegenerate) + " mana";
        anim.SetTrigger("Open");
    }

    private void OnHealthPotionTaken(object healthTaken) {
        feedback.color = feedbackColors.healthPotion;
        feedback.text = "+" + ((int)((float)healthTaken)) + " " + LocalizationManager.GetLocalizedText("SHIP_TEXT_FEEDBACK_HP");
        anim.SetTrigger("Open");

        lastPlayerLife += (float)healthTaken;
    }

    private void OnGoldCoinTaken(object goldCoin) {
        float value = ((Collectible)goldCoin).value;

        if(value > 0) {
            feedback.color = feedbackColors.goldCoin;
            feedback.text = "+" + ((int)value) + " $";
            anim.SetTrigger("Open");
        }
    }

    private void OnBombTaken(object bombObj) {
        feedback.color = feedbackColors.bomb;
        feedback.text = "+" + ((BombCollectible)bombObj).bombsToRefill + " bombs";
        anim.SetTrigger("Open");
    }

    private void OnPlayerDeath(object useless) {
        feedback.gameObject.SetActive(false);
    }
}
