using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ISpecialPower))]
public class SpecialPower : MonoBehaviour {

    public float mana;
    public float manaCost;
    public float coolDownDuration;

    private float coolDownTimer = 0;
    private ISpecialPower power;

    void Start () {
        power = GetComponent<ISpecialPower>();
    }

	// Update is called once per frame
	void Update () {
        coolDownTimer -= Time.deltaTime;

        if (coolDownTimer < 0 && mana >= manaCost && Input.GetButtonDown("SpecialPower")) {
            power.Activate();
            coolDownTimer = coolDownDuration;
            mana -= manaCost;
        }
	}
}
