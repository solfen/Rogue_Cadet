using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MostWantedScreenUI : MonoBehaviour {
    [SerializeField] private Text text;

    private Animator anim;

    void Awake () {
        anim = GetComponent<Animator>();
        EventDispatcher.AddEventListener(Events.GAME_LOADED, OnLoaded);
        EventDispatcher.AddEventListener(Events.GAME_STARTED, OnGameStart);
        EventDispatcher.AddEventListener(Events.BOSS_BEATEN, OnBossBeaten);
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.GAME_LOADED, OnLoaded);
        EventDispatcher.RemoveEventListener(Events.GAME_STARTED, OnGameStart);
        EventDispatcher.RemoveEventListener(Events.BOSS_BEATEN, OnBossBeaten);
    }

    //called from UpgradeShop
    public void OpenGameScene() {
        StartCoroutine(OpenGameSceneRoutine());
    }

    private void OnLoaded(object useless) {
        text.text = "Press start or space";
    }

    private void OnGameStart(object useless) {
        anim.SetTrigger("Close");
    }

    private void OnBossBeaten(object useless) {
        Debug.Log("caca");
        StartCoroutine(BossBeatenRoutine());
    }

    IEnumerator OpenGameSceneRoutine() {
        anim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(0.5f);
        EventDispatcher.DispatchEvent(Events.SCENE_CHANGED, 4);
        SceneManager.LoadScene(4);
    }

    IEnumerator BossBeatenRoutine() {
        yield return new WaitForSecondsRealtime(0.65f);
        Time.timeScale = 0;
        anim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(0.5f);

        while (!Input.GetButtonDown("Start")) {
            yield return null;
        }

        anim.SetTrigger("Close");
        Time.timeScale = 1;
    }
}
