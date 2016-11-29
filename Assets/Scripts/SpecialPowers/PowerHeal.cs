using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHeal : MonoBehaviour, ISpecialPower {

    public float pvAmount;
    public Player target;

    public void Activate() {
        //target.currentLife = Mathf.Min(target.baseMaxLife, target.currentLife + pvAmount);
    }
}
