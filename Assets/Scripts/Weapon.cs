using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShotConfig {
    public Bullet bulletPrefab;
    public Transform origin;
    public float angle;
    public bool targetPlayer;
}

public class Weapon : MonoBehaviour {

    public bool autoFire;
    public bool useController = false;
    public float shotInterval = 0.25f;
    public float damageMultiplier = 1;
    public List<ShotConfig> bullets = new List<ShotConfig>();
    public AudioSource sound;

    private bool isTriggerd = false;
    private float timer;
    private Transform player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
    void Update () {
        isTriggerd = useController && Input.GetButton("MainShot");

        if (timer <= 0 && (autoFire || isTriggerd)) {
            for(int i = 0; i < bullets.Count; i++) {
                Bullet bullet = Instantiate(bullets[i].bulletPrefab, bullets[i].origin.position, Quaternion.Euler(0, 0, bullets[i].angle-90)) as Bullet;
                bullet.Init(bullets[i].origin.rotation.eulerAngles.z + bullets[i].angle, bullets[i].targetPlayer ? player : null, damageMultiplier);
            }

            if(sound != null) {
                sound.Play();
            }

            timer = shotInterval;
        }

        timer -= Time.deltaTime;
    }
	
}
