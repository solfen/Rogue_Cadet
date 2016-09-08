using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {

    public float speed = 5f;
    public float maxLife = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;

    private Transform _transform;
    private Vector3 direction = Vector3.zero;
    private Animator anim;
    private SpriteRenderer spriteRender;

    private float life;
    private float invincibiltyTimer;

    void Start() {
        _transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        life = maxLife;
    }

	void Update () {
        Move();
        LifeUpdate();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(invincibiltyTimer <= 0) {
            if(other.tag == "Enemy") {
                life -= other.GetComponent<Enemy>().meleeDamage;
            }
            else if(other.tag == "Bullet") {
                life -= other.GetComponent<Bullet>().damage;
            }

            spriteRender.color = Color.red;
            invincibiltyTimer = invicibiltyDuration;
        }
    }

    private void Move() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        _transform.position += direction * speed * Time.deltaTime;

        anim.SetFloat("XSpeed", direction.x);
    }

    private void LifeUpdate() {
        if(life <= 0) {
            anim.SetTrigger("Death");
            spriteRender.color = Color.white;
            Destroy(gameObject, 0.4f);
        }

        invincibiltyTimer -= Time.deltaTime;
        if (invincibiltyTimer < 0) {
            spriteRender.color = Color.white;
        }
    }
}
