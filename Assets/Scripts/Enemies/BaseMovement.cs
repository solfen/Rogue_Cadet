using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour, ISwitchable {

    public float speed;
    protected Rigidbody2D _rigidbody;
    protected Transform _transform;
    protected bool canSwitch = true;

    // Use this for initialization
    protected virtual void Awake () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    public virtual void SwitchState(bool state) {
        if(canSwitch)
            enabled = state;
    }
}
