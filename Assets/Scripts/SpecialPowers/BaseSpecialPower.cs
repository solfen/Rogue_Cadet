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

    void Awake() {
        EventDispatcher.AddEventListener(Events.MANA_POTION_TAKEN, Regenerate);
    }

    protected virtual void Start() {
        GameData gameData = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;
        ShipConfig shipData = gameData.ships[saveData.selectedShip];

        maxMana = gameData.shipBaseStats.maxMana * (shipData.manaPrecent + saveData.manaUpgradeNb * shipData.manaUpgradeRaise);
        mana = maxMana;

        EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_CREATED, this); //to activate the UI
    }

    protected virtual void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.MANA_POTION_TAKEN, Regenerate);
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

    private void Regenerate(object potionObj) {
        mana = Mathf.Min(maxMana, mana + ((ManaPotion)potionObj).manaToRegenerate);
    }
}