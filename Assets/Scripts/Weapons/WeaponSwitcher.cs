using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour {

    public BaseWeapon currentWeapon { get; private set; }

    private Transform _transform;
    private List<BaseWeapon> weapons = new List<BaseWeapon>();
    private int currentWeaponIndex = 0;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        GameData gameData = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;

        for(int i = saveData.selectedWeapons.Count-1; i >= 0; i--) {
            weapons.Add(Instantiate(gameData.weapons[saveData.selectedWeapons[i]], _transform, false) as BaseWeapon);
        }

        // in case the dumbass player has unequiped all his weapons. Preventing from doing so in the shop would be better but more complicated
        if (weapons.Count == 0) 
            weapons.Add(Instantiate(gameData.weapons[0], _transform, false) as BaseWeapon);

        weapons[currentWeaponIndex].Activate();
        currentWeapon = weapons[currentWeaponIndex];
    }
	
	void Update () {
	    if(Time.timeScale != 0 && InputManager.GetButtonDown(InputManager.GameButtonID.SWITCH_WEAPONS)) {
            weapons[currentWeaponIndex].Disable();
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            weapons[currentWeaponIndex].Activate();
            currentWeapon = weapons[currentWeaponIndex];
        }
    }
}
