using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHeal : BaseSpecialPower {

    public float pvAmount;
    public Player target;

    protected override void Activate() {
        //target.currentLife = Mathf.Min(target.baseMaxLife, target.currentLife + pvAmount);
    }
}
