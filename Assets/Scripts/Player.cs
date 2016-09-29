using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
    public Transform sprite;
    public string typeName;
    public float speed = 5f;
    public float maxLife = 100f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public float life;
    public float rotationMinAngle;
    public float rotationDeadZone = 0.5f;
    public Vector2 hitboxPos;
    public Vector2 hitboxSize;

    public float hitboxUpgradeSizeReduce;
    public float lifeUpgradeRaise;

    private Rigidbody2D _rigidBody;
    private Vector3 direction = Vector3.zero;
    private Animator anim;
    private SpriteRenderer spriteRender;
    private bool isDead = false;
    private GraphRoom currentRoom;
    private GraphRoom newRoom;
    private BoxCollider2D _collider;
    private Color initalColor;

    private float invincibiltyTimer;

    void Start() {
        //Mana_Upgrade 
        hitboxSize *= 1 - PlayerPrefs.GetInt("Hitbox_Upgrade", 0) * hitboxUpgradeSizeReduce;
        maxLife += PlayerPrefs.GetInt("Life_Upgrade", 0) * lifeUpgradeRaise;

        if(PlayerPrefs.HasKey("Equiped_Bomb")) {
            Instantiate(World.instance.bombs[PlayerPrefs.GetInt("Equiped_Bomb")], transform.position, transform.rotation, transform);
        }
        Instantiate(World.instance.weapons[PlayerPrefs.GetInt("Equiped_Weapon")], transform.position, transform.rotation, transform);

        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        anim = sprite.GetComponent<Animator>();
        spriteRender = sprite.GetComponent<SpriteRenderer>();
        initalColor = spriteRender.color;
        life = maxLife;
        _collider.offset = hitboxPos;
        _collider.size = hitboxSize;
    }

	void Update () {
        LifeUpdate();
    }

    void FixedUpdate() {
        Turn();
        Move();

        newRoom = World.instance.map[(int)Mathf.Floor(transform.position.x / World.instance.roomBaseSize.x), (int)Mathf.Floor(transform.position.y / World.instance.roomBaseSize.y)].room;
        if(newRoom != currentRoom) {
            MiniMap.instance.OnPlayerEnterRoom(newRoom);
            if(PowerBomb.instance != null) {
                PowerBomb.instance.OnPlayerEnterRoom(newRoom);
            }
        }
        currentRoom = newRoom;
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
            spriteRender.color = initalColor;
        }
    }

    private void Die() {
        anim.SetTrigger("Death");
        spriteRender.color = initalColor;
        isDead = true;
        DeathScreen.instance.OnPlayerDeath();
        Destroy(gameObject, 0.4f);
    }
}
