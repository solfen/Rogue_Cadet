using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;
    public float damage;
    public float maxDistance;

    private Vector2 direction;
    private Rigidbody2D _rigidbody;

	void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        _rigidbody.MovePosition(_rigidbody.position + direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void Init(float angle, Transform target, float dmgMultiplier) {
        damage *= dmgMultiplier;

        if (target != null) {
            direction = target.position - transform.position;
            direction.Normalize();
            transform.rotation = Quaternion.Inverse(target.rotation);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, angle-90);

            angle *= Mathf.Deg2Rad;
            direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        }
    }

    void OnTriggerEnter2D() {
        Destroy(gameObject);
    }
}
