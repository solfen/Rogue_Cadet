using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeleteSave : MonoBehaviour {

    private int timePressed = -1;
    [SerializeField] private int nbToPress = 3;
    [SerializeField] private Text nbToPressText;

	// Use this for initialization
	void Start () {
        OnPressed();
    }
	
    public void OnPressed() {
        timePressed++;

        if(timePressed == nbToPress) {
            FileSaveLoad.Delete();
            PlayerPrefs.DeleteAll(); //tmp only for demo version, where the user changes when the save is destroyed

            Time.timeScale = 1;
            EventDispatcher.DispatchEvent(Events.SCENE_CHANGED, 0);
            SceneManager.LoadScene(0);
            
        }

        int pressLeft = nbToPress - timePressed;
        nbToPressText.text = pressLeft <= 0 ? LocalizationManager.GetLocalizedText("SETTINGS_PANE_DELETED_SAVE_FEEDBACK") : string.Format(LocalizationManager.GetLocalizedText("SETTINGS_PANE_DELETE_SAVE_FEEDBACK"), pressLeft);
    }
}
