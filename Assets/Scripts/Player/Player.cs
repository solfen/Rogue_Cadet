using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameData gameData;
    public Transform sprite;

    public float currentLife;
    public bool isInvisible;
    private float maxLife;

    public float hitboxUpgradeSizeReduce;
    public float lifeUpgradeRaise;
    public float manaUpgradeRaise;

    [SerializeField] private GameObject hitShield;
    [SerializeField] private Dungeon dungeon;

    private float currentInvicibiltyDuration;

    private Transform _transform;
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

        ShipConfig config = gameData.ships[PlayerPrefs.GetInt("SelectedShip", 0)];
        maxLife = gameData.shipBaseStats.maxLife * config.lifePrecent;
        currentLife = maxLife;
        currentInvicibiltyDuration = gameData.shipBaseStats.invicibiltyDuration * config.invicibiltyDurationPercent;
        _collider.size = gameData.shipBaseStats.hitboxSize * config.hitboxSizePercent;

        /*currentHitboxSize *= 1 - PlayerPrefs.GetInt("Hitbox_Upgrade", 0) * hitboxUpgradeSizeReduce;
        baseMaxLife += PlayerPrefs.GetInt("Life_Upgrade", 0) * lifeUpgradeRaise;
        baseMaxMana += PlayerPrefs.GetInt("Mana_Upgrade", 0) * manaUpgradeRaise;*/

        Instantiate(gameData.weapons[PlayerPrefs.GetInt("Equiped_Weapon")], _transform.position, _transform.rotation, _transform);
        Instantiate(config.power, _transform, false);

        initalColor = spriteRender.color;

        EventDispatcher.DispatchEvent(Events.PLAYER_CREATED, this);
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
            SetCurrentRoom(); //tmp
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
        currentLife -= dmg;
        spriteRender.color = Color.red;
        hitShield.SetActive(true);
        invincibiltyTimer = currentInvicibiltyDuration;
    }

    public void Heal(float amount) {
        currentLife = Mathf.Min(maxLife, currentLife + amount);
    }

    private void SetCurrentRoom() {
        newRoom = dungeon.map[(int)Mathf.Floor(_transform.position.x / gameData.roomBaseSize.x), (int)Mathf.Floor(_transform.position.y / gameData.roomBaseSize.y)].room;
        if (newRoom != currentRoom) {
            MiniMap.instance.OnPlayerEnterRoom(newRoom);
            EventDispatcher.DispatchEvent(Events.PLAYER_ENTER_ROOM, newRoom);
        }
        currentRoom = newRoom;
    }

    private void LifeUpdate() {
        if(currentLife <= 0) {
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
