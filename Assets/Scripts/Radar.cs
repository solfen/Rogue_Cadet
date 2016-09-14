using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {

    public GameObject enemiesPointPrefab;
    public Transform pointsParent;
    public float radarRadius;
    public float radarScale;

    private World world;
    private List<GameObject> pointsPool = new List<GameObject>();
    private Vector3 directorVector;
    private int searchIndex;
    private Transform playerTransform;

    void Start() {
        world = World.instance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update() {
        if(playerTransform  == null) {
            enabled = false;
        }

        searchIndex = 0;

        for (int i = 0; i < world.enemies.Count; i++) {
            directorVector = (world.enemies[i].transform.position - playerTransform.position) * radarScale;
            if(directorVector.magnitude < radarRadius) {
                GameObject point = GetUnsuedPoint();
                searchIndex++;
                point.SetActive(true);
                point.transform.localPosition = directorVector;
            }
        }

        for(;  searchIndex < pointsPool.Count; searchIndex++) {
            pointsPool[searchIndex].SetActive(false);
        }
    }

    private GameObject GetUnsuedPoint() {
        if(searchIndex < pointsPool.Count) {
            return pointsPool[searchIndex];
        }

        GameObject point = Instantiate(enemiesPointPrefab, pointsParent, false) as GameObject;
        pointsPool.Add(point);
        return point;
    }
}
