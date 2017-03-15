using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneMusicInfo {
    public int sceneIndex;
    public AudioSource musicSource;
    public float musicVolume;
}

public class MusicManager : MonoBehaviour {

    [SerializeField] private List<SceneMusicInfo> scenesMusicList;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private AnimationCurve fadeOutCurve;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private AnimationCurve fadeInCurve;

    private static MusicManager instance; // yes it's a singleton. But it's private. I just need to make sure I don't have two music managers when changin scenes.
    private Dictionary<int, SceneMusicInfo> scenesMusic = new Dictionary<int, SceneMusicInfo>();
    private int currentScene;

    void Awake() {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for(int i = 0; i < scenesMusicList.Count; i++) {
            scenesMusic.Add(scenesMusicList[i].sceneIndex, scenesMusicList[i]);
        }

        EventDispatcher.AddEventListener(Events.SCENE_CHANGED, OnSceneChange);
        OnSceneChange(SceneManager.GetActiveScene().buildIndex);

        instance = this;
    }

    void OnDestroy() {
        EventDispatcher.RemoveEventListener(Events.SCENE_CHANGED, OnSceneChange);
    }

    private void OnSceneChange(object newSceneIndex) {
        StartCoroutine(SwitchMusic((int)newSceneIndex));
    }
    
    IEnumerator SwitchMusic(int newScene) {
        if(scenesMusic.ContainsKey(currentScene)) {
            yield return StartCoroutine(Fade(scenesMusic[currentScene].musicSource, 0, fadeOutDuration, fadeOutCurve));
            scenesMusic[currentScene].musicSource.Stop();
        }

        if(scenesMusic.ContainsKey(newScene)) {
            scenesMusic[newScene].musicSource.Play();
            StartCoroutine(Fade(scenesMusic[newScene].musicSource, scenesMusic[newScene].musicVolume, fadeInDuration, fadeInCurve));
        }

        currentScene = newScene;
    }

    IEnumerator Fade(AudioSource source, float endVolume, float duration, AnimationCurve curve) {
        float startVolume = source.volume;

        for (float t = 0; t < duration; t += Time.unscaledDeltaTime) {
            source.volume = Mathf.Lerp(startVolume, endVolume, curve.Evaluate(t / duration));
            yield return null;
        }

        source.volume = endVolume;
    }
}
