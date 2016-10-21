using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour {

    public float speed;
    protected Rigidbody2D _rigidbody;
    protected Transform _transform;

    // Use this for initialization
    void Awake () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }
}
