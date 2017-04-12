using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour {
    [SerializeField] private Score score;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScore;
    [SerializeField] private Text time;
    [SerializeField] private Text bestTime;
    [SerializeField] private Text kills;
    [SerializeField] private Text bestKills;
    [SerializeField] private Text tip;
    [SerializeField] private List<string> deathTips;
    [SerializeField] private int levelToLoad = 1;
    [SerializeField] private bool loadSceneOnExit = true;

    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, OnPlayerDeath);
    }

    void Update () {
        if(Input.GetButtonDown("Start")) {
            Time.timeScale = 1;
            Input.ResetInputAxes();
            EventDispatcher.DispatchEvent(Events.SCENE_CHANGED, levelToLoad);
            if(loadSceneOnExit) {
                SceneManager.LoadScene(levelToLoad);
            }

            enabled = false;
        }
    }

    private void OnPlayerDeath(object useless) {
        anim.enabled = true;
        enabled = true;
        tip.text = LocalizationManager.GetLocalizedText(deathTips[Random.Range(0, deathTips.Count)]); 

        if (score != null) {
            scoreText.text = LocalizationManager.GetLocalizedText("DEATH_SCREEN_MONEY") + ((int)score.score) + "$";
            time.text = LocalizationManager.GetLocalizedText("DEATH_SCREEN_TIME") + FormatTime(Time.time - score.timeStart);
            kills.text = LocalizationManager.GetLocalizedText("DEATH_SCREEN_KILLS") + score.enemiesKilled;

            bestScore.text = LocalizationManager.GetLocalizedText("DEATH_SCREEN_BEST") + ((int)Mathf.Max(score.score, GlobalData.instance.saveData.highScore)) + "$";
            bestTime.text = LocalizationManager.GetLocalizedText("DEATH_SCREEN_BEST") + FormatTime(Mathf.Max(Time.time - score.timeStart, GlobalData.instance.saveData.bestTime));
            bestKills.text = LocalizationManager.GetLocalizedText("DEATH_SCREEN_BEST") + Mathf.Max(score.enemiesKilled, GlobalData.instance.saveData.bestEnemiesKilled);
        }
        else {
            scoreText.text = "";
            time.text = "";
            kills.text = "";
            bestScore.text = "";
            bestTime.text = "";
            bestKills.text = "";
        }
    }

    private string FormatTime(float seconds) {
        System.TimeSpan ts = System.TimeSpan.FromSeconds(seconds);
        return string.Format("{0}:{1}:{2}", ((int)ts.TotalHours), ts.Minutes, ts.Seconds);
    }
}
