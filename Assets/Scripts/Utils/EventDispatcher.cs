using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Events {
    COLLECTIBLE_TAKEN,
    ENEMY_DIED,
    PLAYER_DIED,
    PLAYER_HIT,
    PLAYER_ENTER_ROOM
}

public static class EventDispatcher {

    public delegate void EventHandler(object emiter);
    private static Dictionary<Events, EventHandler> events = new Dictionary<Events, EventHandler> { //are values kept between scenes ?
        { Events.COLLECTIBLE_TAKEN,  null },
        { Events.ENEMY_DIED,  null },
        { Events.PLAYER_DIED,  null },
        { Events.PLAYER_HIT,  null },
        { Events.PLAYER_ENTER_ROOM,  null },

    };

    public static void AddEventListener(Events type, EventHandler method) {
        events[type] += method;
    }

    public static void RemoveEventListener(Events type, EventHandler method) { // actually, I need to test that...
        events[type] -= method;
    }

    public static void DispatchEvent(Events type, object sender) {
        if(events[type] != null) {
            events[type](sender);
        }
    }
}
