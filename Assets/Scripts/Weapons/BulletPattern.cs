using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BulletPattern : ScriptableObject {
    [Header("General")]
    public float startDelay = 0; // not sure about that
    public int bulletsNb;
    public float volleyInterval;
    [Tooltip("The base angle is toward the player instead of the up transform")]
    public bool targetPlayer;

    [Header("Bullets Loop")]
    public float angleStart;
    public float delayBetweenBullets;
    public float angleBetweenBullets;
    public float angleMin;
    public float angleMax;
    public float angleMultiplierAtMax = -1;
}
