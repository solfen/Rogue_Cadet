using UnityEngine;
using System.Collections;

public class ControllerMovement : BaseMovement {

    public float rotationDeadZone = 0.5f;

    [SerializeField] private Animator anim;

    private Rigidbody2D _rigidBody;
    private Vector3 direction = Vector3.zero;

    void Start() {
        _rigidBody = GetComponent<Rigidbody2D>(); // tmp Base Movement will take care of it
    }

    void FixedUpdate() {
        Move();
        Turn();
    }

    private void Move() {
        direction.x = Input.GetAxisRaw("MoveX");
        direction.y = Input.GetAxisRaw("MoveY");

        _rigidBody.velocity = direction * speed;

        anim.SetFloat("XSpeed", direction.x);
    }

    private void Turn() {
        if (InputManager.useGamedad) {
            direction.x = Input.GetAxisRaw("AimX");
            direction.y = -Input.GetAxisRaw("AimY");
        }
        else {
            direction = Input.mousePosition - Camera.main.WorldToScreenPoint(_transform.position);
        }

        if (!InputManager.useGamedad || direction.x > rotationDeadZone || direction.x < -rotationDeadZone || direction.y > rotationDeadZone || direction.y < -rotationDeadZone) {
            _rigidBody.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        }
    }
}
