using UnityEngine;
using System.Collections;
using System;

public class PowerVampirism : BaseSpecialPower {

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float distance;
    [SerializeField] private float duration;
    [SerializeField] private float dps;
    [SerializeField] private float healConversion;

    private Transform playerTrans;
    private Player player;
    private int layerMask = 1 << 11;
    private Collider2D[] enemies = new Collider2D[10];

    protected override void Start() {
        base.Start();
        playerTrans = transform.parent;
        player = playerTrans.GetComponent<Player>();
    }

    protected override void Activate() {
        StartCoroutine(Vampire());
    }

    IEnumerator Vampire() {
        float timer = duration;
        particles.Play();
        while (timer > 0) {
            int enemiesNb = Physics2D.OverlapCircleNonAlloc(playerTrans.position, distance, enemies, layerMask);
            for(int i = 0; i < enemiesNb; i++) {
                enemies[i].GetComponent<Enemy>().Hit(dps * Time.deltaTime);
            }

            player.Heal(dps * Time.deltaTime * enemiesNb * healConversion);

            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
