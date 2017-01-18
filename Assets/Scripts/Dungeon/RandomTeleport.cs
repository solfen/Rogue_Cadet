using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTeleport : MonoBehaviour, IInteractable {

    [SerializeField] private Teleporter associatedTeleporter;

    private List<Vector3> availablePositions = new List<Vector3>();
    private Transform _transform;

	// Use this for initialization
	void Awake () {
        EventDispatcher.AddEventListener(Events.TELEPORTER_CREATED, OnTeleporterCreated);
        EventDispatcher.AddEventListener(Events.GAME_STARTED, OnGameLoaded);
        _transform = GetComponent<Transform>();
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.TELEPORTER_CREATED, OnTeleporterCreated);
        EventDispatcher.RemoveEventListener(Events.GAME_STARTED, OnGameLoaded);
    }

    private void OnTeleporterCreated(object TeleporterPos) {
        Vector3 pos = (Vector3)TeleporterPos;
        if (pos != _transform.position) {
            availablePositions.Add(pos);
        }

    }

    private void OnGameLoaded(object useless) {
        if(availablePositions.Count < 1) {
            Destroy(gameObject);
            return;
        }
    }

    public void Activate() {
        associatedTeleporter.SetTeleportPos(availablePositions[Random.Range(0, availablePositions.Count)]);
        associatedTeleporter.Activate();
    }
}
