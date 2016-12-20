using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResetGame : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("cac");
            PlayerPrefs.SetInt("Hitbox_Upgrade", 0);
            PlayerPrefs.SetInt("Life_Upgrade", 0);
            PlayerPrefs.SetInt("Mana_Upgrade", 0);
            PlayerPrefs.SetInt("Equiped_Weapon", 0);
            PlayerPrefs.DeleteKey("Equiped_Bomb");
            PlayerPrefs.SetString("selectedShip", "Knight");
            PlayerPrefs.SetFloat("Money", 0);

            SceneManager.LoadScene(0);
        }
	}
}
