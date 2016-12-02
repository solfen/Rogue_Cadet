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
        data.hitboxUpgradeNb += 30;
        FileSaveLoad.Save(data);
        test.TestStuff();
    }
}
