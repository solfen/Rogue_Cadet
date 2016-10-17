using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


    [HideInInspector]
    public Bullet nextAvailable = null; // for the pool linked list
    [HideInInspector]
    public float damage;

    private Vector2 direction;
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private Bullet prefab;
    private float speed;
    private bool destroyOutScreen;

	void Awake () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    public void Init(BulletStats stats, Vector3 pos, Vector2 dir) {
        _transform.parent = stats.parent;
        prefab = stats.prefab;
        damage = stats.damage;
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

    void OnBecameInvisible () {
        if (gameObject.activeSelf && destroyOutScreen) {
            gameObject.SetActive(false);
            BulletsFactory.BulletDeath(prefab, this);
        }
    }

    void OnTriggerEnter2D () {
        gameObject.SetActive(false);
        BulletsFactory.BulletDeath(prefab, this);
    }

    IEnumerator KillTime(float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        BulletsFactory.BulletDeath(prefab, this);
    }
}
