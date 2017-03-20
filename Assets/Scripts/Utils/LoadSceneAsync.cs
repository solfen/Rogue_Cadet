using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneAsync : MonoBehaviour, IInteractable {

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

        if(allowSceneActivationDirect) {
            EventDispatcher.DispatchEvent(Events.SCENE_CHANGED, sceneIndex);
        }
    }

    public void AllowActivate() {
        EventDispatcher.DispatchEvent(Events.SCENE_CHANGED, sceneIndex);
        loader.allowSceneActivation = true;
    }

    public void Activate() {
        AllowActivate();
    }

}
