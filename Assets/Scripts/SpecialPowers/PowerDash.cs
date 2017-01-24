using UnityEngine;
using System.Collections;

public class PowerDash : BaseSpecialPower {

    [SerializeField] private float speedMultiplier = 10;
    [SerializeField] private float dashDuration = 0.25f;

    private BaseMovement playerMovement;
    private Player player;

    protected override void Start() {
        base.Start();
        playerMovement = transform.parent.GetComponent<BaseMovement>();
        player = transform.parent.GetComponent<Player>();
    }

    protected override void Activate() {
        StartCoroutine(Dash());
    }

    IEnumerator Dash() {
        playerMovement.speed *= speedMultiplier;
        player.invincibiltyTimer = dashDuration;
        yield return new WaitForSeconds(dashDuration);
        playerMovement.speed /= speedMultiplier;
    }
}
