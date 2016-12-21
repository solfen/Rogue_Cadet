using UnityEngine;
using System.Collections;

public class SaveCurrentSceneIndex : MonoBehaviour {

    [SerializeField] private int sceneIndex;
	// Use this for initialization
	void Start () {
        SaveData data = FileSaveLoad.Load();
        data.currentScene = sceneIndex;
        FileSaveLoad.Save(data);
    }
}
