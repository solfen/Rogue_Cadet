using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {

    public Camera cam;
    public float speed = 5f;
    public float maxLife = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public float life;

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
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        life = maxLife;
    }

	void Update () {
        Move();
        LifeUpdate();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Damage(other);
    }
    void OnTriggerStay2D(Collider2D other) {
        Damage(other);
    }

    private void Damage(Collider2D other) {
        if (invincibiltyTimer <= 0) {
            if (other.tag == "Enemy") {
                life -= other.GetComponent<Enemy>().meleeDamage;
            }
            else if (other.tag == "Bullet") {
                life -= other.GetComponent<Bullet>().damage;
            }

            spriteRender.color = Color.red;
            invincibiltyTimer = invicibiltyDuration;
        }
    }

    private void Move() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        newPos = _transform.position + direction * speed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, cam.orthographicSize * cam.aspect+2f, world.worldUnitysize.x - cam.orthographicSize * cam.aspect - 2f);
        newPos.y = Mathf.Clamp(newPos.y, cam.orthographicSize+0.5f, world.worldUnitysize.y - cam.orthographicSize - 0.5f);

        _transform.position = newPos;

        anim.SetFloat("XSpeed", direction.x);
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
