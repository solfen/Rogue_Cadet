using UnityEngine;
using System.Collections;
using System;

public class PowerInvisibility : BaseSpecialPower {

    [SerializeField] private float duration;
    private Player player;
    private SpriteRenderer playerRenderer;

    protected override void Start() {
        base.Start();
        player = transform.parent.GetComponent<Player>();
        playerRenderer = player.sprite.GetComponent<SpriteRenderer>();
    }

    protected override void Activate() {
        StartCoroutine(Invisibility());
    }

    IEnumerator Invisibility() {
        player.isInvisible = true;
        playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 0.5f);
        yield return new WaitForSeconds(duration);
        player.isInvisible = false;
        playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 1);
    }
}
