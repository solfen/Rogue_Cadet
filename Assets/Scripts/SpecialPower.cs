using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower : MonoBehaviour {

    public float maxMana;
    public float manaCost;
    public float coolDownDuration;

    [HideInInspector]
    public float mana;

    private float coolDownTimer = 0;
    private ISpecialPower power;

    void Start () {
        power = GetComponent<ISpecialPower>();
        mana = maxMana;
    }

	// Update is called once per frame
	void Update () {
        coolDownTimer -= Time.deltaTime;

        if (coolDownTimer < 0 && mana >= manaCost && Input.GetButtonDown("SpecialPower")) {
            power.Activate();
            coolDownTimer = coolDownDuration;
            mana -= manaCost;
            Debug.Log("caca");
            PowerUI.instance.OnUsePower(this);
        }
	}
}
