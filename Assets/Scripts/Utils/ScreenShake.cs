﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScreenShakeConfig {
    public ScreenShakeTypes type;
    public float duration;
    public float amplitude;
    public AnimationCurve curve;
}

public enum ScreenShakeTypes {
    NONE,
    EXPLOSION,
    SHOT,
    PLAYER_DEATH,
    PLAYER_HIT
}

public class ScreenShake : MonoBehaviour {

    [SerializeField] private float shakeInterval = 0.1f;
    [SerializeField] private List<ScreenShakeConfig> configsList;

    private Dictionary<ScreenShakeTypes, ScreenShakeConfig> configs = new Dictionary<ScreenShakeTypes, ScreenShakeConfig>();
    private Transform _transform;
    private float amplitudeMultiplier;


    void Awake () {
        for(int i = 0; i < configsList.Count; i++) {
            configs.Add(configsList[i].type, configsList[i]);
        }

        _transform = GetComponent<Transform>();
        OnAmplitudeModifierChanged(PlayerPrefs.GetFloat("ScreenShakeFore"));

        EventDispatcher.AddEventListener(Events.ENEMY_DIED, StartExplosionShake);
        EventDispatcher.AddEventListener(Events.BULLET_VOLLEY_FIRED, StartFireShake);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, StartPlayerDeathShake);
        EventDispatcher.AddEventListener(Events.PLAYER_HIT, StartPlayerHitShake);
        EventDispatcher.AddEventListener(Events.SCREEN_SHAKE_MODIFIER_CHANGED, OnAmplitudeModifierChanged);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, StartExplosionShake);
        EventDispatcher.RemoveEventListener(Events.BULLET_VOLLEY_FIRED, StartFireShake);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, StartPlayerDeathShake);
        EventDispatcher.RemoveEventListener(Events.PLAYER_HIT, StartPlayerHitShake);
        EventDispatcher.RemoveEventListener(Events.SCREEN_SHAKE_MODIFIER_CHANGED, OnAmplitudeModifierChanged);
    }

    private void OnAmplitudeModifierChanged(object value) {
        amplitudeMultiplier = (float)value;
    }

    private void StartFireShake(object bulletFountain) {
        BulletFountain fountain = (BulletFountain)bulletFountain;
        if(fountain.screenShakeType != ScreenShakeTypes.NONE) {
            StartCoroutine(Shake(configs[ScreenShakeTypes.SHOT], -fountain.transform.up));
        }
    }

    public void StartExplosionShake(object enemy) {
        StartCoroutine(Shake(configs[ScreenShakeTypes.EXPLOSION], null, ((Enemy)enemy).shakeAmplitudeMultiplier));
    }

    public void StartPlayerDeathShake(object useless) {
        StartCoroutine(Shake(configs[ScreenShakeTypes.PLAYER_DEATH]));
    }

    public void StartPlayerHitShake(object useless) {
        StartCoroutine(Shake(configs[ScreenShakeTypes.PLAYER_HIT]));
    }

    IEnumerator Shake(ScreenShakeConfig config, Vector3? dir = null, float intensityMultiplier = 1) {
        float timer = 0;
        float lastTime = Time.unscaledTime;

        do {
            Vector3 random = dir != null ? (Vector3)dir : (Vector3)Random.insideUnitCircle;
            _transform.position += random * config.curve.Evaluate(timer / config.duration) * config.amplitude * intensityMultiplier * amplitudeMultiplier;

            yield return new WaitForSecondsRealtime(shakeInterval);

            timer += Time.unscaledTime - lastTime;
            lastTime = Time.unscaledTime;

        } while (timer < config.duration);
    }
}
