using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;
    public float damage;
    public float maxDistance;

    private Vector3 direction;
    private Vector3 initialPos;
    private Transform _transform;

	void Start () {
        _transform = GetComponent<Transform>();
        initialPos = _transform.position;
    }

    void Update() {
        _transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(initialPos, _transform.position) >= maxDistance) {
            Destroy(gameObject);
        }
    }

    public void Init(float angle, Transform target) {
        if(target != null) {
            direction = target.position - transform.position;
            direction.Normalize();
        }
        else {
            angle *= Mathf.Deg2Rad;
            direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        }
    }

    void OnTriggerEnter2D() {
        Destroy(gameObject);
    }
}
