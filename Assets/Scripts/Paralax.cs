using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Zone {
    public string name;
    [Tooltip("In number of backgrounds")]
    public Vector2 position;
    public Vector2 size;
    public GameObject backgroundsPrefab;
}

public class Paralax : MonoBehaviour {

    public Transform player;
    public List<Zone> zones = new List<Zone>();
    public Vector2 backgroundSize;

    private Transform[] currentBackgrounds = new Transform[4];
    private List<Transform> backgrounds = new List<Transform>();
    private float numberOfPaneXOffset;
    private float numberOfPaneYOffset;
    private float previousXoffset = -9000; // to be sure the first time will initiate the backgrounds
    private float previousYoffset = -9000;
    private Vector3[] backgroundPosition = new Vector3[4];

    void Start () {
        for(int i=0; i<zones.Count; i++) {
            zones[i].size.Scale(backgroundSize); // set the size to Unity units instead of nb of backgrounds size;
            zones[i].position.Scale(backgroundSize); // set the size to Unity units instead of nb of backgrounds size;

            for (int j=0; j<4; j++) {
                GameObject bg = Instantiate(zones[i].backgroundsPrefab, transform) as GameObject;
                bg.SetActive(false);
                backgrounds.Add(bg.transform);
            }
        }

        for(int i=0; i<4; i++) {
            currentBackgrounds[i] = backgrounds[i]; //inits the currentBackgrounds just in case 
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null) {
            return;
        }

        UpdateBackground();
    }

    private void UpdateBackground() {
        numberOfPaneXOffset = Mathf.Floor(player.position.x / backgroundSize.x);
        numberOfPaneYOffset = Mathf.Floor((player.position.y + backgroundSize.y / 2) / backgroundSize.y);

        if (previousXoffset != numberOfPaneXOffset || previousYoffset != numberOfPaneYOffset) {
            backgroundPosition[0].Set(numberOfPaneXOffset * backgroundSize.x, numberOfPaneYOffset * backgroundSize.y, 0);
            backgroundPosition[1].Set((numberOfPaneXOffset + 1) * backgroundSize.x, numberOfPaneYOffset * backgroundSize.y, 0);
            backgroundPosition[2].Set(numberOfPaneXOffset * backgroundSize.x, (numberOfPaneYOffset + 1) * backgroundSize.y, 0);
            backgroundPosition[3].Set((numberOfPaneXOffset + 1) * backgroundSize.x, (numberOfPaneYOffset + 1) * backgroundSize.y, 0);

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < zones.Count; j++) {
                    if (backgroundPosition[i].x >= zones[j].position.x && backgroundPosition[i].x <= zones[j].position.x + zones[j].size.x
                    && backgroundPosition[i].y >= zones[j].position.y && backgroundPosition[i].y <= zones[j].position.y + zones[j].size.y) {
                        currentBackgrounds[i].gameObject.SetActive(false);
                        currentBackgrounds[i] = backgrounds[j * 4 + i]; // backgrounds are ordered by zone in the list so it's: number of zones * total of bg + current bg index
                        currentBackgrounds[i].position = backgroundPosition[i];
                        currentBackgrounds[i].gameObject.SetActive(true);

                        break;
                    }
                }
            }

        }

        previousXoffset = numberOfPaneXOffset;
        previousYoffset = numberOfPaneYOffset;
    }
}
