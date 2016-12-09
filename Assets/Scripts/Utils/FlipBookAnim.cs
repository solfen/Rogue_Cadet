using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlipBookAnim : MonoBehaviour {

    [SerializeField] private SpriteRenderer target;
    [SerializeField] private float frameDuration;
    [SerializeField] private bool autoPlay;
    [SerializeField] private bool loop;
    [SerializeField] List<Sprite> sprites;

    private int spriteIndex = 0;
    private IEnumerator routine = null;

    void Start () {
        if(autoPlay) {
            Play();
        }
    }

    public void Play() {
        if(routine == null) {
            routine = FlipSprites();
            StartCoroutine(routine);
        }
    }

    public void Stop() {
        if(routine != null) {
            StopCoroutine(routine);
            routine = null;
        }
    }

    IEnumerator FlipSprites() {
        while(spriteIndex < sprites.Count) {
            target.sprite = sprites[spriteIndex];
            spriteIndex = loop ? (spriteIndex + 1) % sprites.Count : spriteIndex + 1;

            float timer = frameDuration;
            while(timer > 0) {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }
    
}
