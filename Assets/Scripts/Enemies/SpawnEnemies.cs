using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour, ISwitchable {

    public Enemy enemyPrefab;
    public int nbToSpawn;
    public float spawnInterval;
    public float spawnZoneRadius;

    private float timer;
    private int nbSpawned;
    private Vector3 spawnPos = new Vector3();
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= spawnInterval) {
            timer = 0;

            spawnPos.Set(transform.position.x + Random.Range(-spawnZoneRadius, spawnZoneRadius), transform.position.y + Random.Range(-spawnZoneRadius, spawnZoneRadius), 0);
            Transform enemy = Instantiate(enemyPrefab).GetComponent<Transform>();
            enemy.position = spawnPos;
            enemy.parent = transform.parent;

            nbSpawned++;
            if (nbSpawned >= nbToSpawn) {
                enabled = false;
                return;
            }
        }

    }

    public void SwitchState(bool state) {
        enabled = state;
    }
}
