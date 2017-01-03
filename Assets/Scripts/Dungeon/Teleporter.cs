using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour, IInteractable {

    [SerializeField] private bool isTeleportBack;
    [SerializeField] private float animDuration = 1;
    [SerializeField] private Vector3 teleportPos;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Animator screenTransitorAnim;

    private Transform playerTransform;

	void Awake () {
        EventDispatcher.AddEventListener(Events.PLAYER_CREATED, OnPlayerCreation);

        if(isTeleportBack) {
            EventDispatcher.AddEventListener(Events.PLAYER_TELEPORTED, OnPlayerTP);
        }
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_CREATED, OnPlayerCreation);
        EventDispatcher.RemoveEventListener(Events.PLAYER_TELEPORTED, OnPlayerTP);
    }

    private void OnPlayerCreation(object _player) {
        playerTransform = ((Player)_player).GetComponent<Transform>();
    }

    private void OnPlayerTP (object playerLastPos) {
        teleportPos = (Vector3)playerLastPos;
    }

    public void Activate() {
        StartCoroutine("Teleport");
    }

    IEnumerator Teleport() {
        Vector3 playerPos = playerTransform.position;

        particles.Play();
        yield return new WaitForSeconds(animDuration);
        screenTransitorAnim.SetTrigger("Transition");
        yield return new WaitForSeconds(0.2f);

        playerTransform.position = teleportPos;
        EventDispatcher.DispatchEvent(Events.PLAYER_TELEPORTED, playerPos); // needs to be after to avoid teleport back autoInfluence

        Destroy(gameObject, 1);
    }
}
