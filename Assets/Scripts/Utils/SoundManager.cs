using System.Collections;
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
    WEAPON_SHOT_1
}

[System.Serializable]
public class GenericSound {
    public GenericSoundsEnum key;
    public AudioSource value;
}

public class SoundManager : MonoBehaviour {

    public List<GenericSound> soundsList;
    public static SoundManager instance;

    private Dictionary<GenericSoundsEnum, AudioSource> sounds = new Dictionary<GenericSoundsEnum, AudioSource>();

	void Awake () {
        instance = this;
        for(int i = 0; i < soundsList.Count; i++) {
            sounds.Add(soundsList[i].key, soundsList[i].value);
        }

        EventDispatcher.AddEventListener(Events.BULLET_VOLLEY_FIRED, PlayBulletSound);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.BULLET_VOLLEY_FIRED, PlayBulletSound);
    }

    private void PlayBulletSound(object fountain) {
        GenericSoundsEnum sound = ((BulletFountain)fountain).volleySound;
        if (sound != GenericSoundsEnum.NONE)
            PlaySound(sound);
    }
	
    public void PlaySound(GenericSoundsEnum sound) {
        sounds[sound].Play();
    }
}
