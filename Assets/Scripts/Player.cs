using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
    public Transform sprite;
    public float speed = 5f;
    public float maxLife = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public float life;
    public float rotationMinAngle;
    public float rotationDeadZone = 0.5f;

    private Rigidbody2D _rigidBody;
    private Vector3 direction = Vector3.zero;
    private Vector3 newPos;
    private Animator anim;
    private SpriteRenderer spriteRender;
    private bool isDead = false;

    private float invincibiltyTimer;

    void Start() {
        _rigidBody = GetComponent<Rigidbody2D>();
        anim = sprite.GetComponent<Animator>();
        spriteRender = sprite.GetComponent<SpriteRenderer>();
        life = maxLife;
    }

	void Update () {
        LifeUpdate();
    }

    void FixedUpdate() {
        Turn();
        Move();
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch(other.tag) {
            case "Enemy":
                Damage(other.gameObject.GetComponent<Enemy>().meleeDamage);
            break;

            case "Bullet":
                Damage(other.gameObject.GetComponent<Bullet>().damage);
            break;

        }

        World.instance.Score.PlayerHit();
    }

    void OnTriggerStay2D(Collider2D other) {
        OnTriggerEnter2D(other);
    }

    private void Damage(float dmg) {
        if (invincibiltyTimer <= 0) {
            life -= dmg;
            spriteRender.color = Color.red;
            invincibiltyTimer = invicibiltyDuration;
        }
    }

    private void Move() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        _rigidBody.velocity = direction * speed;

        anim.SetFloat("XSpeed", direction.x);
    }

    private void Turn() {
        direction.x = -Input.GetAxisRaw("Horizontal2");
        direction.y = -Input.GetAxisRaw("Vertical2");

        if(direction.x > rotationDeadZone || direction.x < -rotationDeadZone || direction.y > rotationDeadZone || direction.y < -rotationDeadZone) {
            _rigidBody.rotation = Mathf.Floor((Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) / rotationMinAngle) * rotationMinAngle;
        }
    }

    private void LifeUpdate() {
        if(life <= 0 && !isDead) {
            Die();
        }

        invincibiltyTimer -= Time.deltaTime;
        if (invincibiltyTimer < 0) {
            spriteRender.color = Color.white;
        }
    }

    private void Die() {
        anim.SetTrigger("Death");
        spriteRender.color = Color.white;
        Destroy(gameObject, 0.4f);
        isDead = true;
    }
}
