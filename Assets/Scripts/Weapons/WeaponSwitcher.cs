using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour {

    public Weapon currentWeapon { get; private set; }

    private Transform _transform;
    private List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0;

	// Use this for initialization
	void Start () {
        _transform = GetComponent<Transform>();
        GameData gameData = GlobalData.instance.gameData;
        SaveData saveData = GlobalData.instance.saveData;

        for(int i = saveData.selectedWeapons.Count-1; i >= 0; i--) {
            weapons.Add(Instantiate(gameData.weapons[saveData.selectedWeapons[i]], _transform, false) as Weapon);
        }

        weapons[currentWeaponIndex].Activate();
        currentWeapon = weapons[currentWeaponIndex];
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("SwitchWeapon")) {
            weapons[currentWeaponIndex].Disable();
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            weapons[currentWeaponIndex].Activate();
            currentWeapon = weapons[currentWeaponIndex];
        }
    }
}
