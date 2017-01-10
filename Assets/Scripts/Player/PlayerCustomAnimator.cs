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
            if(previousAnim != null)
                animations[previousAnim].Stop();

            animations[currentAnim].Play();
            previousAnim = currentAnim;
        }
    }

    public void PlayExplosion() {
        for (int i = 0; i < animationsList.Count; i++) {
            animationsList[i].anim.Stop();
        }

        enabled = false;

        animations["Explosion"].Play();
    }
}
