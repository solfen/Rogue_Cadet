﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float meleeDamage;
    public float life;
    public float hitFeedbakcDuration;

    private SpriteRenderer spriteRender;
    private Animator anim;
    private Color initialColor;

    void Start() {
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialColor = spriteRender.color;
        StartCoroutine("LifeUpdate");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(life <= 0) {
            return;
        }

        if (other.tag == "Player") {
            life -= other.GetComponent<Player>().meleeDamage;
        }
        else if (other.tag == "PlayerBullet") {
            life -= other.GetComponent<Bullet>().damage;
        }

        StartCoroutine("hitFeedback");
    }

    IEnumerator hitFeedback() {
        float timer = hitFeedbakcDuration;
        while(timer > 0) {
            spriteRender.color = Color.red;
            yield return null;
            timer -= Time.deltaTime;
            spriteRender.color = Color.white;
            yield return null;
            timer -= Time.deltaTime;
        }

        spriteRender.color = initialColor;
    }

    IEnumerator LifeUpdate() {
        while(true) {
            if(life <= 0) {
                StopCoroutine("hitFeedback");

                anim.SetTrigger("Death");
                spriteRender.color = Color.white;
                Destroy(gameObject, 0.4f);

                StopCoroutine("LifeUpdate");
            }

            yield return null;
        }
    } 
}
