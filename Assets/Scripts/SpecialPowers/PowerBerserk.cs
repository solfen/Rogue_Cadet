using UnityEngine;
using System.Collections;

public class PowerBerserk : BaseSpecialPower {

    public float duration;

    private Player player;
    private SpriteRenderer playerRenderer;
    private Weapon playerWeapon;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        player = transform.parent.GetComponent<Player>();
        playerRenderer = player.sprite.GetComponent<SpriteRenderer>();
        playerWeapon = player.GetComponentInChildren<Weapon>();
	}
	
    public override void Activate() {
        StartCoroutine(BerserkTime());
    }

    IEnumerator BerserkTime() {
        Color initialColor = playerRenderer.color;
        float initialFireDuration = playerWeapon.maxFireDuration;

        player.transform.localScale *= 1.5f;
        playerRenderer.color = Color.red;
        playerWeapon.maxFireDuration = 9000;
        playerWeapon.coolDownTimer = -1;

        float timer = duration;
        while (timer > 0) {
            yield return null; 
            timer -= Time.unscaledDeltaTime;
        }

        player.transform.localScale /= 1.5f;
        playerRenderer.color = initialColor;
        playerWeapon.maxFireDuration = initialFireDuration;
    }
}
