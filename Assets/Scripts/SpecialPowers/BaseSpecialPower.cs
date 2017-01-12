using UnityEngine;
using System.Collections;

public abstract class BaseSpecialPower : MonoBehaviour {

    [SerializeField] 
    public float manaCost;
    public float coolDownDuration;

    [HideInInspector] public float coolDownTimer = 0;
    [HideInInspector] public float mana;
    [HideInInspector] public float maxMana;

    protected abstract void Activate();

    protected virtual void Start() {
        GameData gameData = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;
        ShipConfig shipData = gameData.ships[saveData.selectedShip];

        maxMana = gameData.shipBaseStats.maxMana * (shipData.manaPrecent + saveData.manaUpgradeNb * shipData.manaUpgradeRaise);
        mana = maxMana;

        EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_CREATED, this); //to activate the UI
    }

    protected virtual void Update() {
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

    public virtual void Regenerate(float amount) {
        mana = Mathf.Min(maxMana, mana + amount);
    }
}