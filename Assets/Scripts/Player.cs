using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Camera cam;
    public Transform sprite;
    public float speed = 5f;
    public float maxLife = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public float life;
    public float rotationMinAngle;

    private World world;
    private Transform _transform;
    private Vector3 direction = Vector3.zero;
    private Vector3 newPos;
    private Animator anim;
    private SpriteRenderer spriteRender;
    private bool isDead = false;

    private float invincibiltyTimer;

    void Start() {
        world = World.instance;
        _transform = GetComponent<Transform>();
        anim = sprite.GetComponent<Animator>();
        spriteRender = sprite.GetComponent<SpriteRenderer>();
        life = maxLife;
    }

	void Update () {
        Turn();
        GetMoveDirection();
        Move();
        LifeUpdate();
    }

    void OnTriggerEnter2D(Collider2D other) {
        switch(other.tag) {
            case "Enemy":
                Damage(other.GetComponent<Enemy>().meleeDamage);
            break;

            case "Bullet":
                Damage(other.GetComponent<Bullet>().damage);
            break;

            case "Wall":
            Debug.Log("Hello");
                direction *= -1;
                Move();
            break;
        }
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

    private void GetMoveDirection() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
    }

    private void Move() {
        newPos = _transform.position + direction * speed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, cam.orthographicSize * cam.aspect+2f, world.worldUnitysize.x - cam.orthographicSize * cam.aspect - 2f);
        newPos.y = Mathf.Clamp(newPos.y, cam.orthographicSize+0.5f, world.worldUnitysize.y - cam.orthographicSize - 0.5f);

        _transform.position = newPos;

        anim.SetFloat("XSpeed", direction.x);
    }

    private void Turn() {
        direction.x = -Input.GetAxisRaw("Horizontal2");
        direction.y = -Input.GetAxisRaw("Vertical2");

        if(direction.x > 0.5 || direction.x < -0.5 || direction.y > 0.5 || direction.y < -0.5) {
            sprite.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Floor((Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) / rotationMinAngle) * rotationMinAngle));
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
        cam.transform.parent = null;
        Destroy(gameObject, 0.4f);
        isDead = true;
    }
}
