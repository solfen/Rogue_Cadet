using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour {

    [SerializeField] private Text header;
    [SerializeField] private Slider UISlider;
    [SerializeField] private Slider cooldownBar;
    [SerializeField] private float cooldownBarBlinkInterval;

    private Weapon weapon;

	// Use this for initialization
	void Awake () {
        EventDispatcher.AddEventListener(Events.WEAPON_READY, OnWeaponReady);
        enabled = false;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.WEAPON_READY, OnWeaponReady);
    }

    private void OnWeaponReady(object weaponObj) {
        weapon = (Weapon)weaponObj;
        header.text = weapon.displayName + " cooldown:";

        enabled = true;
    }

    // Update is called once per frame
    void Update () {
        if(weapon.isCoolDown) {
            StartCoroutine(CoolDown());
            enabled = false;
            return;
        }
        
        UISlider.value = Mathf.Max(0, 1 - weapon.fireTimer / weapon.maxFireDuration);
	}

    IEnumerator CoolDown() {
        float timer = 0;
        cooldownBar.gameObject.SetActive(true);

        while (weapon.isCoolDown) {
            if(timer < 0) {
                cooldownBar.value = cooldownBar.value == 1 ? 0 : 1;
                timer = cooldownBarBlinkInterval;
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        cooldownBar.gameObject.SetActive(false);
        enabled = true;
    }
}
