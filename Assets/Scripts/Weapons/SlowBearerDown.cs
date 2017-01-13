using UnityEngine;
using System.Collections;

public class SlowBearerDown : MonoBehaviour {

    public float speedMultiplier;

    [SerializeField]
    private BaseWeapon weapon;
    [SerializeField]
    private BaseMovement bearer;

    private bool isSlow = false;
	
    void Start() {
        if(bearer == null) {
            bearer = transform.parent.parent.GetComponent<BaseMovement>();
        }
    }
	// Update is called once per frame
	void Update () {
	    if(weapon.isFiring && !isSlow) {
            bearer.speed *= speedMultiplier;
            isSlow = true;
        }
        else if(!weapon.isFiring && isSlow) {
            bearer.speed /= speedMultiplier;
            isSlow = false;
        }
	}
}
