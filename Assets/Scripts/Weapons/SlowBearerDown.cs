using UnityEngine;
using System.Collections;

public class SlowBearerDown : MonoBehaviour {

    public float firingSpeedMultiplier;

    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private BaseMovement bearer;

    private bool isSlow = false;
	
    void Start() {
        if(bearer == null) {
            bearer = transform.parent.GetComponent<BaseMovement>();
        }
    }
	// Update is called once per frame
	void Update () {
	    if(weapon.isFiring && !isSlow) {
            bearer.speed *= firingSpeedMultiplier;
            isSlow = true;
        }
        else if(!weapon.isFiring && isSlow) {
            bearer.speed /= firingSpeedMultiplier;
            isSlow = false;
        }
	}
}
