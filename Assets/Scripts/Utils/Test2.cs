﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour {

    public Test test;
    private SaveData data;

    void Start() {
        data = FileSaveLoad.Load();
        Debug.Log(data.shipsStock[0]);
    }
}
