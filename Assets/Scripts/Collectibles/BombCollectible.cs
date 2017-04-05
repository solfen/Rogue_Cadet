using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollectible : MonoBehaviour {
    public int bombsToRefill = 1;

    void OnTriggerEnter2D(Collider2D other) {
        EventDispatcher.DispatchEvent(Events.BOMB_COLLECTIBLE_TAKEN, this);
    }
}
