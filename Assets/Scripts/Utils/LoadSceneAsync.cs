using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAsync : MonoBehaviour {

    public int sceneIndex;

    [SerializeField] private bool autoLoad = true;
    [SerializeField] private bool allowSceneActivationDirect = false;

    private AsyncOperation loader;

    // Use this for initialization
    void Start () {
        if(autoLoad) {
            Load();
        }
    }

    public void Load() {
        loader = SceneManager.LoadSceneAsync(sceneIndex);
        loader.allowSceneActivation = allowSceneActivationDirect;
    }

    public void AllowActivate() {
        loader.allowSceneActivation = true;
    }

}
