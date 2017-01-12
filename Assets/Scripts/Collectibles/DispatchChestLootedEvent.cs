using UnityEngine;
using System.Collections;

public class DispatchChestLootedEvent : MonoBehaviour {

    void OnDestroy () {
        EventDispatcher.DispatchEvent(Events.CHEST_LOOTED, null);
    }
}
