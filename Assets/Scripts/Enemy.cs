using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public float meleeDamage;
    public float life;
    public float score;

    [SerializeField]
    private float hitFeedbakcDuration;
    [SerializeField]
    private float spriteTintDuration = 0.16f;

    private World world;
    private SpriteRenderer spriteRender;
    private Animator anim;
    private Color initialColor;

    void Start() {
        world = World.instance;
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialColor = spriteRender.color;
        world.enemies.Add(this);
        StartCoroutine("LifeUpdate");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            life -= other.GetComponent<Player>().meleeDamage;
            StartCoroutine("hitFeedback");
        }
        else if (other.tag == "PlayerBullet") {
            life -= other.GetComponent<Bullet>().damage;
            StartCoroutine("hitFeedback");
        }

    }

    IEnumerator hitFeedback() {
        float timer = hitFeedbakcDuration;

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
            yield return new WaitForSeconds(spriteTintDuration);
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(spriteTintDuration);
        }
    }

    IEnumerator LifeUpdate() {
        while(true) {
            if(life <= 0) {
                StopCoroutine("TintSprite");
                StopCoroutine("hitFeedback");

                anim.SetTrigger("Death");
                spriteRender.color = Color.white;
                world.enemies.Remove(this);
                world.Score.KilledEnemy(this);
                GetComponent<Rigidbody2D>().simulated = false; //remove from physics
                Destroy(gameObject, 0.4f);

                StopCoroutine("LifeUpdate");
            }

            yield return null;
        }
    } 
}
