using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float maxLife;

    private Transform _transform;
    private Vector3 initialPos;
    private Quaternion initialRot;
    private float life;
    private bool isInit = false;

	void Awake () {
        if (!isInit)
            Init();
    }

    private void Init() {
        _transform = GetComponent<Transform>();
        initialPos = _transform.localPosition;
        initialRot = _transform.localRotation;
        life = maxLife;
        isInit = true;
    }

    public void Activate() {
        if (!isInit)
            Init();

        _transform.localPosition = initialPos;
        _transform.localRotation = initialRot;
        life = maxLife;
        gameObject.SetActive(true);
    }

    void Update () {
        _transform.RotateAround(_transform.parent.position, Vector3.forward, moveSpeed * Time.deltaTime);
        _transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
	}

    void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damager = other.GetComponent<DamageDealer>();
        if (damager != null) {
            life -= damager.damage;
            if(life <= 0) {
                gameObject.SetActive(false);
            }
        }
    }
}
