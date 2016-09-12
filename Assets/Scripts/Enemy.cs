using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float meleeDamage;
    public float life;
    public float hitFeedbakcDuration;

    private SpriteRenderer spriteRender;

    void Start() {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            life -= other.GetComponent<Player>().meleeDamage;
        }
        else if (other.tag == "PlayerBullet") {
            life -= other.GetComponent<Bullet>().damage;
        }

        StartCoroutine(hitFeedback());
    }

    IEnumerator hitFeedback() {
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(hitFeedbakcDuration);
        spriteRender.color = Color.white;
    }

    void Update() {
        if(life <= 0) {
            Destroy(gameObject);
        }
    }

    
}
