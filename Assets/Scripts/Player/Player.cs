using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Transform sprite;

    public float currentLife;
    public bool isInvisible;
    public float maxLife;

    [SerializeField] private GameObject hitShield;
    [SerializeField] private Dungeon dungeon;

    private float currentInvicibiltyDuration;

    private GameData gameData;
    private Transform _transform;
    private PlayerCustomAnimator anim;
    private SpriteRenderer spriteRender;
    private bool isDead = false;
    private GraphRoom currentRoom;
    private GraphRoom newRoom;
    private BoxCollider2D _collider;
    private Color initalColor;
    private float difficultyLifeMultiplier;

    public float invincibiltyTimer;

    void Start() {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<BoxCollider2D>();
        anim = sprite.GetComponent<PlayerCustomAnimator>();
        spriteRender = sprite.GetComponent<SpriteRenderer>();
        difficultyLifeMultiplier = PlayerPrefs.GetFloat("PlayerLifeMultiplier", 1);

        gameData = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;
        ShipConfig config = gameData.ships[saveData.selectedShip];

        maxLife = gameData.shipBaseStats.maxLife * (config.lifePrecent + saveData.lifeUpgradeNb * config.lifeUpgradeRaise) * difficultyLifeMultiplier;
        _collider.size = gameData.shipBaseStats.hitboxSize * (config.hitboxSizePercent - saveData.hitboxUpgradeNb * config.hitboxUpgradeRaise);
        currentInvicibiltyDuration = gameData.shipBaseStats.invicibiltyDuration * config.invicibiltyDurationPercent;

        currentLife = maxLife;

        initalColor = spriteRender.color;

        EventDispatcher.AddEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);
        EventDispatcher.DispatchEvent(Events.PLAYER_CREATED, this);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.DIFFICULTY_CHANGED, DifficultyChanged);
    }

    public void Init(Dungeon _dungeon) {
        dungeon = _dungeon;
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
            if(damager.damage >= 0) {
                Damage(damager.damage);
                EventDispatcher.DispatchEvent(Events.PLAYER_HIT, this);
            }
            else {
                Heal(-damager.damage);
            }
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
        GraphRoom newRoom = dungeon.GetRoomFromPosition(_transform.position);
        if(newRoom != null && newRoom != currentRoom) {
            EventDispatcher.DispatchEvent(Events.PLAYER_ENTER_ROOM, newRoom);
            currentRoom = newRoom;
        }
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
        anim.PlayExplosion();
        isDead = true;

        hitShield.SetActive(false);
        spriteRender.color = Color.white;
        _transform.localScale *= 3;
        Time.timeScale = 0.2f;
        EventDispatcher.DispatchEvent(Events.PLAYER_DIED, null);
        Destroy(gameObject, 2);
    }

    private void DifficultyChanged(object useless) {
        float newMultiplier = PlayerPrefs.GetFloat("PlayerLifeMultiplier", 1);
        maxLife = newMultiplier * maxLife / difficultyLifeMultiplier;
        currentLife = newMultiplier * currentLife / difficultyLifeMultiplier;
        difficultyLifeMultiplier = newMultiplier;
    }
}
