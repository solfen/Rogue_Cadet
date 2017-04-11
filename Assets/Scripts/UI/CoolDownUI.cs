using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour {

    [SerializeField] private Text header;
    [SerializeField] private Slider UISlider;
    [SerializeField] private Slider cooldownBar;
    [SerializeField] private float cooldownBarBlinkInterval;

    private BaseWeapon weapon;

	void Awake () {
        EventDispatcher.AddEventListener(Events.WEAPON_READY, OnWeaponReady);
        EventDispatcher.AddEventListener(Events.WEAPON_COOLDOWN_START, OnWeaponCoolDownStart);
        EventDispatcher.AddEventListener(Events.WEAPON_COOLDOWN_END, OnWeaponCoolDownEnd);
        enabled = false;
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.WEAPON_READY, OnWeaponReady);
        EventDispatcher.RemoveEventListener(Events.WEAPON_COOLDOWN_START, OnWeaponCoolDownStart);
        EventDispatcher.RemoveEventListener(Events.WEAPON_COOLDOWN_END, OnWeaponCoolDownEnd);
    }

    private void OnWeaponReady(object weaponObj) {
        weapon = (BaseWeapon)weaponObj;
        header.text = weapon.displayName + " cooldown:";

        cooldownBar.gameObject.SetActive(weapon.coolDownTimer > 0);
        enabled = true;
    }

    void Update () {
        UISlider.value = Mathf.Min(1, weapon.fireTimer / weapon.maxFireDuration);
	}

    private void OnWeaponCoolDownStart(object useless) {
        cooldownBar.gameObject.SetActive(true);
        StartCoroutine("BlinkCooldownBar");
    }

    private void OnWeaponCoolDownEnd(object useless) {
        cooldownBar.gameObject.SetActive(weapon.coolDownTimer > 0); // need to check since it could be a event from a non active weapon
        StopCoroutine("BlinkCooldownBar");
    }

    IEnumerator BlinkCooldownBar() {
        while (true) {
            yield return new WaitForSeconds(cooldownBarBlinkInterval);
            cooldownBar.value = cooldownBar.value == 1 ? 0 : 1;
        }
    }
}