using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float speed;
    public float damage;

    private Vector3 direction;
    private Transform _transform;

	void Start () {
        _transform = GetComponent<Transform>();
    }

    void Update() {
        _transform.position += direction * speed * Time.deltaTime;
    }

    public void Init(float angle) {
        angle *= Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
    }
}
