using UnityEngine;
using System.Collections;

public class ChooseWhichSceneToPlayFirst : MonoBehaviour {

    [SerializeField] private LoadSceneAsync loader;

	// Use this for initialization
	void Start () {
        loader.sceneIndex = FileSaveLoad.Load().currentScene;
        loader.Load();
    }
	
}
