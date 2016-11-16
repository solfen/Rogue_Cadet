using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameData gameData;
    public Transform sprite;
    public string typeName;
    public float speed = 5f;
    public float maxLife = 100f;
    public float maxMana = 100f;
    public float damageMultiplier = 1f;
    public float meleeDamage = 10f;
    public float invicibiltyDuration = 1f;
    public float life;
    public Vector2 hitboxPos;
    public Vector2 hitboxSize;

    public float hitboxUpgradeSizeReduce;
    public float lifeUpgradeRaise;
    public float manaUpgradeRaise;

    [SerializeField] private GameObject hitShield;

    private Transform _transform;
    private Dungeon dungeon;
    private Animator anim;
    private SpriteRenderer spriteRender;
    private bool isDead = false;
    private GraphRoom currentRoom;
    private GraphRoom newRoom;
    private BoxCollider2D _collider;
    private Color initalColor;

    private float invincibiltyTimer;

    void Start() {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<BoxCollider2D>();
        anim = sprite.GetComponent<Animator>();
        spriteRender = sprite.GetComponent<SpriteRenderer>();

        hitboxSize *= 1 - PlayerPrefs.GetInt("Hitbox_Upgrade", 0) * hitboxUpgradeSizeReduce;
        maxLife += PlayerPrefs.GetInt("Life_Upgrade", 0) * lifeUpgradeRaise;
        maxMana += PlayerPrefs.GetInt("Mana_Upgrade", 0) * manaUpgradeRaise;
        if (PlayerPrefs.HasKey("Equiped_Bomb")) {
            Instantiate(gameData.bombs[PlayerPrefs.GetInt("Equiped_Bomb")], _transform.position, _transform.rotation, _transform);
        }
        Instantiate(gameData.weapons[PlayerPrefs.GetInt("Equiped_Weapon")], _transform.position, _transform.rotation, _transform);

        initalColor = spriteRender.color;
        life = maxLife;
        _collider.offset = hitboxPos;
        _collider.size = hitboxSize;

        EventDispatcher.DispatchEvent(Events.PLAYER_CREATED, this);
    }

    public void Init(Dungeon dungeonRef) {
        Debug.Log(dungeonRef);
        dungeon = dungeonRef;
    }

	void Update () {
        if(!isDead)
            LifeUpdate();

        /*if(Input.GetButtonDown("Cheat")) {
            GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
        }*/
    }

    void FixedUpdate() {
        if(dungeon != null)//tmp
            SetCurrentRoom();
    }

    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damager = other.GetComponent<DamageDealer>();
        if (invincibiltyTimer <= 0 && damager != null) {
            Damage(damager.damage);
            EventDispatcher.DispatchEvent(Events.PLAYER_HIT, null);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        OnTriggerEnter2D(other);
    }

    private void Damage(float dmg) {
        life -= dmg;
        spriteRender.color = Color.red;
        hitShield.SetActive(true);
        invincibiltyTimer = invicibiltyDuration;
    }

    private void SetCurrentRoom() {
        newRoom = dungeon.map[(int)Mathf.Floor(_transform.position.x / gameData.roomBaseSize.x), (int)Mathf.Floor(_transform.position.y / gameData.roomBaseSize.y)].room;
        if (newRoom != currentRoom) {
            MiniMap.instance.OnPlayerEnterRoom(newRoom);
            if (PowerBomb.instance != null) {
                PowerBomb.instance.OnPlayerEnterRoom(newRoom);
            }
            EventDispatcher.DispatchEvent(Events.PLAYER_ENTER_ROOM, newRoom);
        }
        currentRoom = newRoom;
    }

    private void LifeUpdate() {
        if(life <= 0) {
            Die();
            return;
        }

        invincibiltyTimer -= Time.deltaTime;
        if (invincibiltyTimer < 0 && hitShield.activeSelf) {
            spriteRender.color = initalColor;
            hitShield.SetActive(false);
        }
    }

    private void Die() {
        anim.SetTrigger("Death");
        StartCoroutine(DieAnim());
        isDead = true;
    }

    IEnumerator DieAnim() {
        yield return new WaitForSeconds(0.2f);
        hitShield.SetActive(false);
        spriteRender.color = Color.white;
        _transform.localScale *= 3;
        Time.timeScale = 0.2f;
        EventDispatcher.DispatchEvent(Events.PLAYER_DIED, null);
        Destroy(gameObject, 2);
    }
}
