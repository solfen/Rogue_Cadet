using UnityEngine;
using System.Collections;
using System;

public class PowerRemoveHitbox : BaseSpecialPower {

    [SerializeField] private float duration;
    [SerializeField] private float goBackSpeed = 5;
     
    private BoxCollider2D playerCollider;
    private SpriteRenderer playerRenderer;
    private int layerMask = 1 << 13;

    protected override void Start() {
        base.Start();
        playerCollider = transform.parent.GetComponent<BoxCollider2D>();
        playerRenderer = transform.parent.GetComponent<Player>().sprite.GetComponent<SpriteRenderer>();
    }

    protected override void Activate() {
        StartCoroutine(RemoveHitbox());
    }

    IEnumerator RemoveHitbox() {
        Vector3 currentSafeRoomPos = playerCollider.transform.position;
   
        playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 0.5f);
        playerCollider.enabled = false;

        yield return new WaitForSeconds(duration);

        playerCollider.enabled = true;
        playerRenderer.color = new Color(playerRenderer.color.r, playerRenderer.color.g, playerRenderer.color.b, 1);

        yield return new WaitForFixedUpdate();
        Vector3 toPreviousRoom = (currentSafeRoomPos - playerCollider.transform.position).normalized;

        while (playerCollider.IsTouchingLayers(layerMask)) {
            playerCollider.transform.position += toPreviousRoom * goBackSpeed;
            yield return new WaitForFixedUpdate();
        }
    }

}
