using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower : MonoBehaviour {

    public float manaCost;
    public float coolDownDuration;
    public string button;
    public bool isBomb = false; //tmp

    [HideInInspector]
    public float mana;
    public float maxMana;

    public float coolDownTimer = 0;
    private ISpecialPower power;

    void Start () {
        power = GetComponent<ISpecialPower>();
        if(!isBomb) {
            maxMana = transform.parent.GetComponent<Player>().maxMana; //tmp
        }
        mana = maxMana;
        NotifyUI();
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
            PowerUI.instance.OnUsePower(this);
        }
    }
}
