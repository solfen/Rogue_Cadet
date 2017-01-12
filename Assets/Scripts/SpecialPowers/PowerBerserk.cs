using UnityEngine;
using System.Collections;

public class PowerBerserk : BaseSpecialPower {

    public float duration;

    private Player player;
    private SpriteRenderer playerRenderer;
    private WeaponSwitcher playerWeaponSwitcher;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        player = transform.parent.GetComponent<Player>();
        playerRenderer = player.sprite.GetComponent<SpriteRenderer>();
        playerWeaponSwitcher = player.GetComponentInChildren<WeaponSwitcher>();
	}
	
    protected override void Activate() {
        StartCoroutine(BerserkTime());
    }

    IEnumerator BerserkTime() {
        Color initialColor = playerRenderer.color;
        Weapon playerWeapon = playerWeaponSwitcher.currentWeapon;

        player.transform.localScale *= 1.5f;
        playerRenderer.color = Color.red;
        playerWeapon.maxFireDuration *= 9000; // 9000 is just my "infinity" value. 'cause you know over nine thousand.
        playerWeapon.coolDownTimer = -1; // reset cooldown if ther's any

        float timer = duration;
        while (timer > 0) {
            yield return null; 
            timer -= Time.unscaledDeltaTime;
        }

        player.transform.localScale /= 1.5f;
        playerRenderer.color = initialColor;
        playerWeapon.maxFireDuration /= 9000;

        EventDispatcher.DispatchEvent(Events.SPECIAL_POWER_USE_END, null);
    }
}
