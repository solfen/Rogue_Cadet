using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerFart : BaseSpecialPower {

    [SerializeField] private List<AudioSource> fartNoise;

    protected override void Activate() {
        fartNoise[Random.Range(0, fartNoise.Count)].Play();
    }

}
