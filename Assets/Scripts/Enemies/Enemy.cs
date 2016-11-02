using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public float life;
    public float score;
    public GenericSoundsEnum explosionSound;
    public SpawnEnemies deathSpawn;

    [SerializeField] private float hitFeedbackDuration;
    [SerializeField] private float spriteColorChangeDuration = 0.16f;
    [SerializeField] private Color tintColor;
    [SerializeField] private Collectible drop;

    private SpriteRenderer spriteRender;
    private Animator anim;
    private Color initialColor;

    void Start() {
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialColor = spriteRender.color;
    }

    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damager = other.GetComponent<DamageDealer>();
        if (damager != null) {
            Hit(damager.damage);
        }
    }

    public void Hit(float damage) {
        life -= damage;
        if (life <= 0) {
            Death();
        }
        else {
            StartCoroutine("hitFeedback");
        }
    }

    IEnumerator hitFeedback() {
        float timer = hitFeedbackDuration;

        StartCoroutine("TintSprite");

        while(timer > 0) {
            timer -= Time.deltaTime;
            yield return null;
        }

        StopCoroutine("TintSprite");
        spriteRender.color = initialColor;
    }

    IEnumerator TintSprite() {
        while(true) {
            spriteRender.color = Color.red;
            yield return new WaitForSeconds(spriteColorChangeDuration);
            spriteRender.color = initialColor;
            yield return new WaitForSeconds(spriteColorChangeDuration);
        }
    }

    private void Death() {
        StopCoroutine("TintSprite");
        StopCoroutine("hitFeedback");

        anim.SetTrigger("Death");
        spriteRender.color = Color.white;
        GetComponent<Rigidbody2D>().simulated = false; //remove from 
        if (drop != null) {
            drop.Pop();
        }
        if(deathSpawn != null) {
            deathSpawn.enabled = true;
        }

        EventDispatcher.DispatchEvent(Events.ENEMY_DIED, this);
        Destroy(gameObject, 0.4f);

        enabled = false;
    }
}
