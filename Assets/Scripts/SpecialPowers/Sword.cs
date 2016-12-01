using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxLife;

    private Transform _transform;
    private float life;
	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        life = maxLife;
    }
	
	// Update is called once per frame
	void Update () {
        _transform.RotateAround(_transform.parent.position, Vector3.forward, moveSpeed * Time.deltaTime);
        _transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
	}

    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damager = other.GetComponent<DamageDealer>();
        if (damager != null) {
            life -= damager.damage;
            if(life <= 0) {
                life = maxLife;
                gameObject.SetActive(false);
            }
        }
    }
}
