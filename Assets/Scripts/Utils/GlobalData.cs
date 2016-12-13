using UnityEngine;
using System.Collections;

public class GlobalData : MonoBehaviour {

    public static GlobalData instance;

    [SerializeField] private GameData _gameData;
    public GameData gameData { get; private set; }
    public SaveData saveData { get; private set; }

	// Use this for initialization
	void Awake () {
        instance = this;
        gameData = _gameData;
        saveData = FileSaveLoad.Load();

        EventDispatcher.AddEventListener(Events.FILE_SAVED, OnFileSaved);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.FILE_SAVED, OnFileSaved);
    }

    private void OnFileSaved(object useless) {
        saveData = FileSaveLoad.Load();
    }
}
