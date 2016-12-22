﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour {
    [SerializeField] private Score score;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScore;
    [SerializeField] private int levelToLoad = 1;

    private Animator anim;
    private AudioSource sound;

    void Awake() {
        sound = GetComponent<AudioSource>();
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
            SceneManager.LoadScene(levelToLoad);
        }
    }

    private void OnPlayerDeath(object useless) {
        anim.enabled = true;
        enabled = true;

        if(score != null) {
            scoreText.text = "Run money: " + score.score + "$";
            highScore.text = "High score: " + Mathf.Max(score.score, GlobalData.instance.saveData.highScore) + "$";
        }
        else {
            scoreText.text = "";
            highScore.text = "";
        }

        sound.Play();
    }
}
