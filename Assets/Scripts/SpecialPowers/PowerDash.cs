using UnityEngine;
using System.Collections;

public class PowerDash : BaseSpecialPower {

    [SerializeField] private float speedMultiplier = 10;
    [SerializeField] private float dashDuration = 0.25f;

    private BaseMovement playerMovement;

    protected override void Start() {
        base.Start();
        playerMovement = transform.parent.GetComponent<BaseMovement>();
    }

    protected override void Activate() {
        StartCoroutine(Dash());
    }

    IEnumerator Dash() {
        playerMovement.speed *= speedMultiplier;
        yield return new WaitForSeconds(dashDuration);
        playerMovement.speed /= speedMultiplier;
    }
}
