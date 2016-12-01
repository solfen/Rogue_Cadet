using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PoweShowTime : BaseSpecialPower {

    [SerializeField] private float duration;
    private Text timeText;

    protected override void Start() {
        base.Start();
        timeText = GameObject.FindGameObjectWithTag("TimeUI").GetComponent<Text>();
    }
    protected override void Activate() {
        StartCoroutine(ShowTime());
    }

    IEnumerator ShowTime() {
        float timer = duration;
        while(timer > 0) {
            timeText.text = System.DateTime.Now.ToString("HH:mm:ss");
            yield return new WaitForSeconds(1);
            timer -= 1;
        }

        timeText.text = "";
    }
}
