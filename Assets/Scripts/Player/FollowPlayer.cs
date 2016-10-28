using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public float aimOffsetDistance;
    public float smoothTime;

    private Transform _transform;
    private Transform playerTransform;
    private float zPos;
    private Vector3 newPos = new Vector3();
    private Vector3 currentAimOffset = new Vector3();
    private Vector3 targetAimOffset = new Vector3();

    private Vector3 velocity = Vector3.zero;

    void Awake() {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        enabled = false;
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    private void OnPlayerCreation(object _player) {
        playerTransform = ((Player)_player).GetComponent<Transform>();
        enabled = true;
    }

    private void OnPlayerDeath(object useless) {
        enabled = false;
    }

    void Start() {
        _transform = GetComponent<Transform>();
        zPos = _transform.position.z;
    }

    void FixedUpdate() {
        if (playerTransform != null) {
            if (InputManager.useGamedad) {
                targetAimOffset.Set(Input.GetAxisRaw("AimX"), -Input.GetAxisRaw("AimY"), 0);
            }
            else {
                float HalfScreenW = Screen.width * 0.5f;
                float HalfScreenH = Screen.height * 0.5f;
                targetAimOffset.Set(Mathf.Clamp((Input.mousePosition.x - HalfScreenW) / HalfScreenW, -1, 1), Mathf.Clamp((Input.mousePosition.y - HalfScreenH) / HalfScreenH, -1, 1), 0);
            }
            currentAimOffset = Vector3.SmoothDamp(currentAimOffset, targetAimOffset * aimOffsetDistance, ref velocity, smoothTime);

            newPos.Set(playerTransform.position.x, playerTransform.position.y, zPos);
            newPos =  newPos + currentAimOffset;

            _transform.position = newPos;
        }
    }
}
