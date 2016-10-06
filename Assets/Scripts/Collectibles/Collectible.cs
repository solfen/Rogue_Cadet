using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    public GenericSoundsEnum sound;
    public float value;

    [SerializeField]
    private int probability;

    public void Pop() {
        if(Random.Range(0,100) < probability) {
            gameObject.SetActive(true);
            transform.parent = null;
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D() {
        World.instance.Score.CollectibleTaken(value);
        SoundManager.instance.PlaySound(sound);
        Destroy(gameObject);
    }
}
