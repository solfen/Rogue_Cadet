using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour {

    public Test test;
    private SaveData data;

    void Start() {
        FileSaveLoad.Save(new SaveData());
        data = FileSaveLoad.Load();
        List<int> selected = new List<int>();
        selected.Add(0); selected.Add(1);
        data.selectedWeapons = selected;
        FileSaveLoad.Save(data);
       // test.TestStuff();
    }
}
