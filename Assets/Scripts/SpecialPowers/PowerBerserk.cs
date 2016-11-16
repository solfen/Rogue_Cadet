using UnityEngine;
using System.Collections;

public class PowerBerserk : MonoBehaviour, ISpecialPower {

    public Player target;
    public float duration;

    private SpriteRenderer targetRenderer;
    private Weapon targetWeapon;

    // Use this for initialization
    void Start () {
        targetRenderer = target.sprite.GetComponent<SpriteRenderer>();
	}
	
    public void Activate() {
        StartCoroutine(BerserkTime());
    }

    IEnumerator BerserkTime() {
        if(targetWeapon == null) {
            targetWeapon = target.GetComponentInChildren<Weapon>();
        }

        Color initialColor = targetRenderer.color;
        float initialFireDuration = targetWeapon.maxFireDuration;

        target.transform.localScale *= 1.5f;
        targetRenderer.color = Color.red;
        targetWeapon.maxFireDuration = 9000;
        targetWeapon.coolDownTimer = -1;

        float timer = duration;
        while (timer > 0) {
            yield return null; 
            timer -= Time.unscaledDeltaTime;
        }

        target.transform.localScale /= 1.5f;
        targetRenderer.color = initialColor;
        targetWeapon.maxFireDuration = initialFireDuration;
    }
}
