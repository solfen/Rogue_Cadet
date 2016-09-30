using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {

    public static DeathScreen instance;
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
        float score = Mathf.Max(PlayerPrefs.GetFloat("HighScore", 0), World.instance.Score.score);
        PlayerPrefs.SetFloat("HighScore", score);
        PlayerPrefs.SetFloat("Money", World.instance.Score.score);
        highScore.text = "High score: " + score + "$";
        sound.Play();
    }
}
