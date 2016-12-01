using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PowerSwordCircle : BaseSpecialPower {

    [SerializeField] private List<Sword> swords;
    private Transform _transform;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        _transform = GetComponent<Transform>();
	}

    protected override void Activate() {
        Debug.Log("wut");
        for(int i = 0; i < swords.Count; i++) {
            swords[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();
        _transform.up = Vector3.up;
	}
}
