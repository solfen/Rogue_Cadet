using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class SaveEmailToTxt : MonoBehaviour {

    public static bool isOpen = false;

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject mailInput;

    void Start() {
        if(PlayerPrefs.GetInt("EmailAsked", 0) == 0) {
            EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
        }
    }
    
    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    void Update() {
        if(isOpen && Input.GetButtonDown("Cancel")) {
            anim.SetTrigger("Close");
            isOpen = false;
        }
    }

    public void OnMailConfirm(string email) {
        File.AppendAllText(Application.persistentDataPath + "emails.txt", email + System.Environment.NewLine);
        anim.SetTrigger("Close");
        isOpen = false;
    }

    private void OnPlayerDeath(object useless) {
        StartCoroutine(WaitAndOpen());
    }

    IEnumerator WaitAndOpen() {
        yield return new WaitForSecondsRealtime(5);
        anim.SetTrigger("Open");
        PlayerPrefs.SetInt("EmailAsked", 1);
        isOpen = true;
        EventSystem.current.SetSelectedGameObject(mailInput);
    }
}
