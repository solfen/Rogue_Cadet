using UnityEngine;
using System.Collections;

public abstract class BaseSpecialPower : MonoBehaviour {

    [SerializeField] private GameData gameData;
    public float manaCost;
    public float coolDownDuration;

    [HideInInspector] public float coolDownTimer = 0;
    [HideInInspector] public float mana;
    [HideInInspector] public float maxMana;

    public abstract void Activate();

    protected virtual void Start() {
        maxMana = gameData.shipBaseStats.maxMana * gameData.ships[PlayerPrefs.GetInt("SelectedShip", 0)].manaPrecent;
        mana = maxMana;

        EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_USED, this); //to activate the UI
        Debug.Log("START");
    }

    void Update() {
        coolDownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("SpecialPower")) {
            if (coolDownTimer < 0 && mana >= manaCost) {
                Activate();
                coolDownTimer = coolDownDuration;
                mana -= manaCost;

                EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_USED, this);
            }
            else {
                EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_USED_IN_COOLDOWN, this);
            }
        }
    }
}