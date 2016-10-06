using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHeal : MonoBehaviour, ISpecialPower {

    public float pvAmount;
    public Player target;

    public void Activate() {
        target.life = Mathf.Min(target.maxLife, target.life + pvAmount);
    }
}
