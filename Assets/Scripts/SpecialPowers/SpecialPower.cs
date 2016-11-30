using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower : MonoBehaviour {

    [SerializeField] private GameData gameData;
    public float manaCost;
    public float coolDownDuration;
    public string button;
    public bool isBomb = false; //tmp

    [HideInInspector]
    public float mana;
    public float maxMana;

    public float coolDownTimer = 0;
    private ISpecialPower power;

    void Start() {
        power = GetComponent<ISpecialPower>();
        if (!isBomb) {
            maxMana = gameData.shipBaseStats.maxMana * gameData.ships[PlayerPrefs.GetInt("SelectedShip", 0)].manaPrecent;
        }
        mana = maxMana;
        NotifyUI();

        EventDispatcher.AddEventListener(Events.GAME_STARTED, OnLoaded);
        enabled = false;
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.GAME_STARTED, OnLoaded);
    }

    private void OnLoaded(object useless) {
        enabled = true;
    }

    // Update is called once per frame
    void Update () {
        coolDownTimer -= Time.deltaTime;

        if(Input.GetButtonDown(button)) {
            if (coolDownTimer < 0 && mana >= manaCost) {
                power.Activate();
                coolDownTimer = coolDownDuration;
                mana -= manaCost;
                NotifyUI();
            }
            else {
                SoundManager.instance.PlaySound(GenericSoundsEnum.ERROR);
            }
        }
	}

    private void NotifyUI() {
        if (isBomb) {
            BombUI.instance.OnUsePower(this);
        }
        else {
            //PowerUI.instance.OnUsePower((BaseSpecialPower)this);
        }
    }
}
