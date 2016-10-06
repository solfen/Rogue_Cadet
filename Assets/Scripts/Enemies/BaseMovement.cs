using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour {

    public float speed;
    protected Transform _transform;

    // Use this for initialization
    void Awake () {
        _transform = GetComponent<Transform>();
    }
}
