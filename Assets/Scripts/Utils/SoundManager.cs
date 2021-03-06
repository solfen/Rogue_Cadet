﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenericSoundsEnum {
    NONE,
    COIN_PICKUP,
    EXPLOSION_1,
    EXPLOSION_2,
    EXPLOSION_3,
    ERROR,
    ACTIVATE,
    WEAPON_SHOT_1,
    PLAYER_EXPLOSION,
    PLAYER_HIT,
    CANT_DO,
    POP_IN,
    POP_OUT,
    SELECT_UI,
    UI_ERROR,
    UI_SUCCESS
}

[System.Serializable]
public class GenericSound {
    public GenericSoundsEnum key;
    public AudioSource value;
}

public class SoundManager : MonoBehaviour {

    public List<GenericSound> soundsList;

    private Dictionary<GenericSoundsEnum, AudioSource> sounds = new Dictionary<GenericSoundsEnum, AudioSource>();

	void Awake () {
        for(int i = 0; i < soundsList.Count; i++) {
            sounds.Add(soundsList[i].key, soundsList[i].value);
        }

        EventDispatcher.AddEventListener(Events.BULLET_VOLLEY_FIRED, PlayBulletSound);
        EventDispatcher.AddEventListener(Events.ENEMY_DIED, PlayExplosionSound);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, PlayPlayerExplosionSound);
        EventDispatcher.AddEventListener(Events.PLAYER_HIT, PlayPlayerHitSound);
        EventDispatcher.AddEventListener(Events.WEAPON_COOLDOWN_START, PlayCantDoSound);
        EventDispatcher.AddEventListener(Events.WEAPON_COOLDOWN_END, PlayActivateSound);
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_COOLDOWN_END, PlayActivateSound);
        EventDispatcher.AddEventListener(Events.SPECIAL_POWER_USED_IN_COOLDOWN, PlayCantDoSound);
        EventDispatcher.AddEventListener(Events.COLLECTIBLE_TAKEN, PlayCollectibleSound);
        EventDispatcher.AddEventListener(Events.OPEN_UI_PANE, PlayOpenPaneSound);
        EventDispatcher.AddEventListener(Events.CLOSE_UI_PANE, PlayClosePaneSound);
        EventDispatcher.AddEventListener(Events.SELECT_UI, PlaySelectUISound);
        EventDispatcher.AddEventListener(Events.UI_ERROR, PlayUIErrorSound);
        EventDispatcher.AddEventListener(Events.UI_SUCCESS, PlayUISuccessSound);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.BULLET_VOLLEY_FIRED, PlayBulletSound);
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, PlayExplosionSound);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, PlayPlayerExplosionSound);
        EventDispatcher.RemoveEventListener(Events.PLAYER_HIT, PlayPlayerHitSound);
        EventDispatcher.RemoveEventListener(Events.WEAPON_COOLDOWN_START, PlayCantDoSound);
        EventDispatcher.RemoveEventListener(Events.WEAPON_COOLDOWN_END, PlayActivateSound);
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_COOLDOWN_END, PlayActivateSound);
        EventDispatcher.RemoveEventListener(Events.SPECIAL_POWER_USED_IN_COOLDOWN, PlayCantDoSound);
        EventDispatcher.RemoveEventListener(Events.COLLECTIBLE_TAKEN, PlayCollectibleSound);
        EventDispatcher.RemoveEventListener(Events.OPEN_UI_PANE, PlayOpenPaneSound);
        EventDispatcher.RemoveEventListener(Events.CLOSE_UI_PANE, PlayClosePaneSound);
        EventDispatcher.RemoveEventListener(Events.SELECT_UI, PlaySelectUISound);
        EventDispatcher.RemoveEventListener(Events.UI_ERROR, PlayUIErrorSound);
        EventDispatcher.RemoveEventListener(Events.UI_SUCCESS, PlayUISuccessSound);
    }

    private void PlayBulletSound(object fountain) {
        GenericSoundsEnum sound = ((BulletFountain)fountain).volleySound;
        if (sound != GenericSoundsEnum.NONE)
            PlaySound(sound);
    }

    private void PlayExplosionSound(object enemy) {
        GenericSoundsEnum sound = ((Enemy)enemy).explosionSound;
        if (sound != GenericSoundsEnum.NONE)
            PlaySound(sound);
    }

    private void PlayPlayerExplosionSound(object useless) {
        PlaySound(GenericSoundsEnum.PLAYER_EXPLOSION);
    }

    private void PlayPlayerHitSound(object useless) {
        PlaySound(GenericSoundsEnum.PLAYER_HIT);
    }

    private void PlayCantDoSound(object useless) {
        PlaySound(GenericSoundsEnum.CANT_DO);
    }

    private void PlayActivateSound(object useless) {
        PlaySound(GenericSoundsEnum.ACTIVATE);
    }

    private void PlayOpenPaneSound(object useless) {
        PlaySound(GenericSoundsEnum.POP_IN);
    }

    private void PlayClosePaneSound(object useless) {
        PlaySound(GenericSoundsEnum.POP_OUT);
    }

    private void PlaySelectUISound(object useless) {
        PlaySound(GenericSoundsEnum.SELECT_UI);
    }

    private void PlayUIErrorSound(object useless) {
        PlaySound(GenericSoundsEnum.UI_ERROR);
    }

    private void PlayUISuccessSound(object useless) {
        PlaySound(GenericSoundsEnum.UI_SUCCESS);
    }

    private void PlayCollectibleSound(object collectibleObj) {
        GenericSoundsEnum sound = ((Collectible)collectibleObj).sound;
        if (sound != GenericSoundsEnum.NONE)
            PlaySound(sound);
    }

    public void PlaySound(GenericSoundsEnum sound) {
        if(!sounds.ContainsKey(sound)) {
            Debug.LogError("Sound:" + sound.ToString() + " doesn't exists");
            return;
        }

        sounds[sound].Play();
    }

    public void PlaySound(int soundIndex) {
        if (soundIndex < 0 && soundIndex >= soundsList.Count) {
            Debug.LogError("Sound index:" + soundIndex + " doesn't exist");
            return;
        }

        soundsList[soundIndex].value.Play();
    }
}
