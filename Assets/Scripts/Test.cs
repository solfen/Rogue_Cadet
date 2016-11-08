using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Test : MonoBehaviour {

    public Material newMat;
    public List<GameObject> objs;

	// Use this for initialization
	void Start () {
        enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < objs.Count; i++) {
            Transform tiled = objs[i].transform.GetChild(0);
            MeshRenderer[] renderers = tiled.GetComponentsInChildren<MeshRenderer>();
            for(int j = 0; j < renderers.Length; j++) {
                renderers[j].material = newMat;
            }
        }

        enabled = false;
        Debug.Log("Mat changed");
	}
}
