using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            Time.timeScale = 1;
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void OnPlayerDeath(float score, float highscore) {
        anim.enabled = true;
        enabled = true;

        scoreText.text = "Run money: " + score + "$";
        highScore.text = "High score: " + highscore + "$";

        sound.Play();
    }
}
