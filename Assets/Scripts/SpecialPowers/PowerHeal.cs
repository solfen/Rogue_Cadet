using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHeal : BaseSpecialPower {

    [SerializeField] private float pvAmount;
    [SerializeField] private ParticleSystem particles;
    private Player player;

    protected override void Start() {
        base.Start();
        player = transform.parent.GetComponent<Player>();
    }

    protected override void Activate() {
        player.Heal(pvAmount);
        particles.Play();
    }
}
