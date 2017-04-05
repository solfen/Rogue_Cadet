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
    BOMB_USED,
    FILE_SAVED,
    REVEAL_TREASURE_MAP,
    PLAYER_TELEPORTED,
    SHIELD_ABSORB_BULLET,
    ACHIEVMENT_CREATED,
    ACHIEVMENT_UNLOCKED,
    SPECIAL_POWER_USE_END,
    SPECIAL_POWER_CREATED,
    SCORE_CHANGED,
    COMBO_CHANGED,
    CHEST_LOOTED,
    TELEPORTER_CREATED,
    OPEN_UI_PANE,
    CLOSE_UI_PANE,
    SELECT_UI,
    UI_ERROR,
    UI_SUCCESS,
    MANA_POTION_TAKEN,
    INPUT_DEVICE_CHANGED,
    SCREEN_SHAKE_MODIFIER_CHANGED,
    DIFFICULTY_CHANGED,
    SCENE_CHANGED,
    SPECIAL_POWER_COOLDOWN_END,
    BOSS_BEATEN,
    COLORBLIND_MODE_CHANGED,
    HEALTH_POTION_TAKEN,
    BOMB_COLLECTIBLE_TAKEN,
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
        { Events.SPECIAL_POWER_USED_IN_COOLDOWN,  null },
        { Events.BOMB_USED,  null },
        { Events.FILE_SAVED,  null },
        { Events.REVEAL_TREASURE_MAP,  null },
        { Events.PLAYER_TELEPORTED,  null },
        { Events.SHIELD_ABSORB_BULLET,  null },
        { Events.ACHIEVMENT_CREATED,  null },
        { Events.ACHIEVMENT_UNLOCKED,  null },
        { Events.SPECIAL_POWER_USE_END,  null },
        { Events.SPECIAL_POWER_CREATED,  null },
        { Events.SCORE_CHANGED,  null },
        { Events.COMBO_CHANGED,  null },
        { Events.CHEST_LOOTED,  null },
        { Events.TELEPORTER_CREATED,  null },
        { Events.OPEN_UI_PANE,  null },
        { Events.CLOSE_UI_PANE,  null },
        { Events.SELECT_UI,  null },
        { Events.UI_ERROR,  null },
        { Events.UI_SUCCESS,  null },
        { Events.MANA_POTION_TAKEN,  null },
        { Events.INPUT_DEVICE_CHANGED,  null },
        { Events.SCREEN_SHAKE_MODIFIER_CHANGED,  null },
        { Events.DIFFICULTY_CHANGED,  null },
        { Events.SCENE_CHANGED,  null },
        { Events.SPECIAL_POWER_COOLDOWN_END,  null },
        { Events.BOSS_BEATEN,  null },
        { Events.COLORBLIND_MODE_CHANGED,  null },
        { Events.HEALTH_POTION_TAKEN,  null },
        { Events.BOMB_COLLECTIBLE_TAKEN,  null },
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
