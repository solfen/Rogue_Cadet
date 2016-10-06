using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

    public static DeathScreen instance;
    public Text scoreText;
    public Text highScore;

    [SerializeField]
    private int levelToLoad = 1;
    private Animator anim;
    private AudioSource sound;

    void Awake() {
        instance = this;
        sound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    void Update () {
        if(Input.GetButtonDown("Start")) {
            InputMapUI.instance.gameObject.SetActive(false);
            Application.LoadLevel(levelToLoad);
        }
    }

    public void OnPlayerDeath() {
        anim.enabled = true;
        enabled = true;
        float score = World.instance.Score.score;
        float highscore = Mathf.Max(PlayerPrefs.GetFloat("HighScore", 0), score);

        PlayerPrefs.SetFloat("HighScore", highscore);
        PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money", 0) + score);

        scoreText.text = "Run money: " + score + "$";
        highScore.text = "High score: " + highscore + "$";

        sound.Play();
    }
}
