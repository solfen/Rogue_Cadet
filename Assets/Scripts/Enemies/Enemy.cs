using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public float life;
    public float score;
    public float shakeAmplitudeMultiplier = 1;
    public GenericSoundsEnum explosionSound;
    public SpawnEnemies deathSpawn;

    [SerializeField] private float hitFeedbackDuration;
    [SerializeField] private float spriteColorChangeDuration = 0.16f;
    [SerializeField] private Color tintColor;
    [SerializeField] private Collectible drop;

    private SpriteRenderer spriteRender;
    private Animator anim;
    private Color initialColor;
    private float difficultyLifeMultiplier;

    void Start() {
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialColor = spriteRender.color;
        difficultyLifeMultiplier = PlayerPrefs.GetFloat("EnemiesLifeMultiplier", 1);
        life *= difficultyLifeMultiplier;

        EventDispatcher.AddEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);
    }

    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damager = other.GetComponent<DamageDealer>();
        if (damager != null) {
            Hit(damager.damage);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        OnTriggerEnter2D(other);
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
        GetComponent<Rigidbody2D>().simulated = false; //remove from physics
        if (drop != null) {
            drop.Pop();
        }
        if(deathSpawn != null) {
            deathSpawn.enabled = true;
        }

        EventDispatcher.DispatchEvent(Events.ENEMY_DIED, this);

        Destroy(gameObject, 0.6f);

        enabled = false;
    }

    private void DifficultyChanged(object useless) {
        float newMultiplier = PlayerPrefs.GetFloat("EnemiesLifeMultiplier", 1);
        life = newMultiplier * life / difficultyLifeMultiplier;
        difficultyLifeMultiplier = newMultiplier;
    }
}
