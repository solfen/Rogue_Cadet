using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Events {
    COLLECTIBLE_TAKEN,
    ENEMY_DIED,
    PLAYER_CREATED,
    PLAYER_DIED,
    PLAYER_HIT,
    PLAYER_ENTER_ROOM,
    BULLET_VOLLEY_FIRED,
    DUNGEON_GRAPH_CREATED,
    GAME_LOADED,
    WEAPON_READY,
    WEAPON_COOLDOWN_START,
    WEAPON_COOLDOWN_END,
    GAME_STARTED,
    SPECIAL_POWER_USED,
    SPECIAL_POWER_USED_IN_COOLDOWN,
}

public static class EventDispatcher {

    public delegate void EventHandler(object emiter);
    private static Dictionary<Events, EventHandler> events = new Dictionary<Events, EventHandler> { //are values kept between scenes ?
        { Events.COLLECTIBLE_TAKEN,  null },
        { Events.ENEMY_DIED,  null },
        { Events.PLAYER_CREATED,  null },
        { Events.PLAYER_DIED,  null },
        { Events.PLAYER_HIT,  null },
        { Events.PLAYER_ENTER_ROOM,  null },
        { Events.BULLET_VOLLEY_FIRED,  null },
        { Events.DUNGEON_GRAPH_CREATED,  null },
        { Events.GAME_LOADED,  null },
        { Events.WEAPON_READY,  null },
        { Events.WEAPON_COOLDOWN_START,  null },
        { Events.WEAPON_COOLDOWN_END,  null },
        { Events.GAME_STARTED,  null },
        { Events.SPECIAL_POWER_USED,  null },
        { Events.SPECIAL_POWER_USED_IN_COOLDOWN,  null }
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
