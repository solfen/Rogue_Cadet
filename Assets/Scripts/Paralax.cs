using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paralax : MonoBehaviour {

    public Transform player;
    public List<GameObject> backgroundsPrefabs = new List<GameObject>();
    public Vector2 backgroundSize; 

    private List<Transform> backgrounds = new List<Transform>();
    private float cameraHeight;
    private float cameraWidth;
    private float numberOfPaneXOffset;
    private float numberOfPaneYOffset;

    void Start () {
	    for(int i=0; i<4; i++) {
            GameObject bg = Instantiate(backgroundsPrefabs[0], transform) as GameObject;
            backgrounds.Add(bg.transform);
        }

        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Camera.main.aspect;

    }
	
	// Update is called once per frame
	void Update () {
        numberOfPaneXOffset = Mathf.Floor(player.position.x / backgroundSize.x);
        numberOfPaneYOffset = Mathf.Floor((player.position.y+backgroundSize.y/2) / backgroundSize.y);

        backgrounds[0].position = new Vector3(numberOfPaneXOffset * backgroundSize.x, numberOfPaneYOffset * backgroundSize.y, 0);
        backgrounds[1].position = new Vector3((numberOfPaneXOffset+1) * backgroundSize.x, numberOfPaneYOffset * backgroundSize.y, 0);
        backgrounds[2].position = new Vector3((numberOfPaneXOffset) * backgroundSize.x, (numberOfPaneYOffset+1) * backgroundSize.y, 0);
        backgrounds[3].position = new Vector3((numberOfPaneXOffset+1) * backgroundSize.x, (numberOfPaneYOffset + 1) * backgroundSize.y, 0);

    }
}
