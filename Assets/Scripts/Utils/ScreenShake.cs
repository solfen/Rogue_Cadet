using UnityEngine;
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
}

public class ScreenShake : MonoBehaviour {

    [SerializeField] private List<ScreenShakeConfig> configsList;

    private Dictionary<ScreenShakeTypes, ScreenShakeConfig> configs = new Dictionary<ScreenShakeTypes, ScreenShakeConfig>();
    private Transform _transform;


    void Awake () {
        for(int i = 0; i < configsList.Count; i++) {
            configs.Add(configsList[i].type, configsList[i]);
        }

        _transform = GetComponent<Transform>();

        EventDispatcher.AddEventListener(Events.ENEMY_DIED, StartExplosionShake);
        EventDispatcher.AddEventListener(Events.BULLET_VOLLEY_FIRED, StartFireShake);
        EventDispatcher.AddEventListener(Events.PLAYER_DIED, StartPlayerDeathShake);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.ENEMY_DIED, StartExplosionShake);
        EventDispatcher.RemoveEventListener(Events.BULLET_VOLLEY_FIRED, StartFireShake);
        EventDispatcher.RemoveEventListener(Events.PLAYER_DIED, StartPlayerDeathShake);
    }

    private void StartFireShake(object bulletFountain) {
        BulletFountain fountain = (BulletFountain)bulletFountain;
        if(fountain.screenShakeType != ScreenShakeTypes.NONE) {
            StartCoroutine(Shake(configs[ScreenShakeTypes.SHOT], -fountain.transform.up));
        }
    }

    public void StartExplosionShake(object useless) {
        StartCoroutine(Shake(configs[ScreenShakeTypes.EXPLOSION]));
    }

    public void StartPlayerDeathShake(object useless) {
        StartCoroutine(Shake(configs[ScreenShakeTypes.PLAYER_DEATH]));
    }

    IEnumerator Shake(ScreenShakeConfig config, Vector3? dir = null) {
        float timer = 0;

        while (timer < config.duration) {
            Vector3 random = dir != null ? (Vector3)dir : (Vector3)Random.insideUnitCircle;
            _transform.position += random * config.curve.Evaluate(timer / config.duration) * config.amplitude;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
