using UnityEngine;
using System.Collections;

public class AttatchCanvasToMainCamera : MonoBehaviour {

    [SerializeField] private Canvas canvas;
    [SerializeField] private float planeDist;
 	// Use this for initialization
	void Start () {
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = planeDist;
    }
}
