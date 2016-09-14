using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Backgrounds : MonoBehaviour {

    public Transform backgroundsParent;

    private World world;
    private Transform player;
    private Transform[] currentBackgrounds = new Transform[4];
    private List<Transform> backgrounds = new List<Transform>();
    private float mapPosX;
    private float mapPosY;
    private float previousPosX = -9000; // to be sure the first time will initiate the backgrounds
    private float previousPosY = -9000;
    private Vector3 backgroundPosition = new Vector3();

    void Start () {
        world = World.instance;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i=0; i< world.zones.Count; i++) {
            for (int j=0; j<4; j++) {
                GameObject bg = Instantiate(world.zones[i].backgroundsPrefab, backgroundsParent) as GameObject;
                bg.SetActive(false);
                backgrounds.Add(bg.transform);
            }
        }

        for(int i=0; i<4; i++) {
            currentBackgrounds[i] = backgrounds[i]; //inits the currentBackgrounds just in case 
            currentBackgrounds[i].gameObject.SetActive(true);
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
        mapPosX = Mathf.Floor((player.position.x - world.backgroundSize.x / 2) / world.backgroundSize.x);
        mapPosY = Mathf.Floor((player.position.y - world.backgroundSize.y / 2) / world.backgroundSize.y);


        if (previousPosX != mapPosX || previousPosY != mapPosY) {
            int cpt = 0;
            for(int i = 0; i < 2; i++) {
                for(int j = 0; j<2; j++) {
                    currentBackgrounds[cpt].gameObject.SetActive(false);
                    int zoneType = world.map[((int)mapPosX) + i, ((int)mapPosY) + j].zoneType; //might be a Index out range exception. Player limits must be set accordingly
                    currentBackgrounds[i] = backgrounds[zoneType * 4 + cpt];
                    backgroundPosition.Set((mapPosX+i) * world.backgroundSize.x, (mapPosY+j) * world.backgroundSize.y, 0);
                    currentBackgrounds[i].position = backgroundPosition;
                    currentBackgrounds[i].gameObject.SetActive(true);
                    cpt++;
                }
            }
        }

        previousPosX = mapPosX;
        previousPosY = mapPosY;
    }
}
