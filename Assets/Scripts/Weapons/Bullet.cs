using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;
    public float damage;
    public float maxDistance;
    public Collider2D _collider;

    [HideInInspector]
    public Bullet nextAvailable = null; // for the pool linked list

    private Vector2 direction;
    private Rigidbody2D _rigidbody;
    private Bullet prefab;

	void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        _rigidbody.MovePosition(_rigidbody.position + direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
        BulletsFactory.BulletDeath(prefab, this);
    }

    public void Init(Bullet _prefab, Transform parent, Vector3 pos, float angle, float dmgMultiplier, Transform target = null) {
        Transform _transform = GetComponent<Transform>();

        _transform.parent = parent;
        _transform.position = pos;
        prefab = _prefab;
        damage *= dmgMultiplier;

        if (target != null) {
            direction = target.position - _transform.position;
            direction.Normalize();
            _transform.rotation = Quaternion.Inverse(target.rotation);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, angle-90);

            angle *= Mathf.Deg2Rad;
            direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        }

        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D() {
        gameObject.SetActive(false);
    }
}
