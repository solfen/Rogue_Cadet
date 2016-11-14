using UnityEngine;
using System.Collections;

public class ControllerMovement : BaseMovement {

    public float rotationDeadZone = 0.5f;

    [SerializeField] private Animator anim;

    private Vector3 direction = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    void FixedUpdate() {
        Move();
        Turn();

        anim.SetFloat("Speed", (rotation + direction).magnitude);
    }

    private void Move() {
        direction.x = Input.GetAxisRaw("MoveX");
        direction.y = Input.GetAxisRaw("MoveY");

        _rigidbody.velocity = direction * speed;

    }

    private void Turn() {
        if (InputManager.useGamedad) {
            rotation.x = Input.GetAxisRaw("AimX");
            rotation.y = -Input.GetAxisRaw("AimY");
        }
        else {
            rotation = Input.mousePosition - Camera.main.WorldToScreenPoint(_transform.position);
        }

        if (!InputManager.useGamedad || rotation.x > rotationDeadZone || rotation.x < -rotationDeadZone || rotation.y > rotationDeadZone || rotation.y < -rotationDeadZone) {
            _rigidbody.rotation = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg - 90;
        }
    }
}
