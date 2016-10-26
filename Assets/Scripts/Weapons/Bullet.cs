﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


    [HideInInspector]
    public Bullet nextAvailable = null; // for the pool linked list

    private Vector2 direction;
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private DamageDealer damager;
    private Bullet prefab;
    private float speed;
    private bool destroyOutScreen;

	void Awake () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        damager = GetComponent<DamageDealer>();
    }

    public void Init(BulletStats stats, Vector3 pos, Vector2 dir) {
        _transform.parent = stats.parent;
        prefab = stats.prefab;
        damager.damage = stats.damage;
        speed = stats.speed;
        destroyOutScreen = stats.destroyOutScreen;

        _transform.position = pos;
        _transform.up = dir;
        direction = dir; 

        gameObject.SetActive(true);
        _rigidbody.velocity = direction * speed;

        if(stats.lifeTime > 0) {
            StartCoroutine(KillTime(stats.lifeTime));
        }
    }

    void Die() {
        gameObject.SetActive(false);
        BulletsFactory.BulletDeath(prefab, this);
    }

    void OnBecameInvisible () {
        if (gameObject.activeSelf && destroyOutScreen) {
            Die();
        }
    }

    void OnTriggerEnter2D () {
        Die();
    }

    IEnumerator KillTime(float time) {
        yield return new WaitForSeconds(time);
        Die();
    }

    public void BombKill() {
        Die();
    }
}
