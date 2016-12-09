using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlipBookAnimationForDictionary {
    public string name;
    public FlipBookAnim anim;
}

public class PlayerCustomAnimator : MonoBehaviour {

    public float playerSpeed;
    [SerializeField] private List<FlipBookAnimationForDictionary> animationsList;

    private Dictionary<string, FlipBookAnim> animations = new Dictionary<string, FlipBookAnim>();
    private string currentAnim = null;
    private string previousAnim = null;

    void Awake () {
        for(int i = 0; i < animationsList.Count; i++) {
            animations.Add(animationsList[i].name, animationsList[i].anim);
        }
    }

    void Update() {
        currentAnim = playerSpeed > 1.2 ? "ThrustMax" : playerSpeed < 0.5 ? "ThrustLow" : "Idle";

        if (previousAnim != currentAnim) {
            previousAnim = currentAnim;
            
            for(int i = 0; i < animationsList.Count; i++) {
                if(animationsList[i].name == currentAnim) {
                    animationsList[i].anim.Play();
                }
                else {
                    animationsList[i].anim.Stop();
                }
            }
        }
    }

    public void PlayExplosion() {
        for (int i = 0; i < animationsList.Count; i++) {
            animationsList[i].anim.Stop();
        }

        animations["Explosion"].Play();
    }


    
}
