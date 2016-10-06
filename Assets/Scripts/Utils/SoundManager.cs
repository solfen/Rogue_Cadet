using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenericSoundsEnum {
    COIN_PICKUP,
    EXPLOSION_1,
    EXPLOSION_2,
    EXPLOSION_3,
    ERROR,
    ACTIVATE
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
    }
	
    public void PlaySound(GenericSoundsEnum sound) {
        sounds[sound].Play();
    }
}
