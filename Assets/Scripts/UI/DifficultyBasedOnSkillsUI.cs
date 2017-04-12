using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyBasedOnSkillsUI : MonoBehaviour {
    
    [SerializeField] private MainMenuPanes difficultyPane;
    [SerializeField] private Animator anim;
    [SerializeField] private Graphic pane;

    void Start () {
        EventDispatcher.AddEventListener(Events.SCENE_CHANGED, OnDeathScreenExit);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.SCENE_CHANGED, OnDeathScreenExit);
    }

    private void OnDeathScreenExit(object useless) {
        StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine() {
        pane.gameObject.SetActive(true);
        anim.SetTrigger("Open");

        for(float t = 0; t < 0.5f; t += Time.unscaledDeltaTime) {
            yield return null;
        }

        difficultyPane.Open();

        while(!Input.GetButtonDown("Start")) {
            yield return null;
        }

        SceneManager.LoadScene(3);
    }

}
