using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    [SerializeField] private bool isTeleportBack;
    [SerializeField] private float animDuration = 1;
    [SerializeField] private Vector3 teleportPos;
    [SerializeField] private GameObject hintBtn;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Animator screenTransitorAnim;

    private Transform playerTransform;

	// Use this for initialization
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

    void OnTriggerEnter2D () {
        StartCoroutine("WaitForInput");
        hintBtn.SetActive(true);
    }

    void OnTriggerExit2D() {
        StopCoroutine("WaitForInput");
        hintBtn.SetActive(false);
    }

    IEnumerator WaitForInput () {
	    while(!Input.GetButtonDown("Submit")) {
            yield return null;
        }

        StartCoroutine(Teleport());
	}

    IEnumerator Teleport() {
        particles.Play();
        yield return new WaitForSeconds(animDuration);
        screenTransitorAnim.SetTrigger("Transition");
        yield return new WaitForSeconds(0.2f);

        EventDispatcher.DispatchEvent(Events.PLAYER_TELEPORTED, playerTransform.position);
        playerTransform.position = teleportPos;
        Destroy(gameObject);
    }
}
