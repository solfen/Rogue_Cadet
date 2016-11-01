using UnityEngine;
using System.Collections;

[System.Serializable]
public class BulletStats {
    [HideInInspector]
    public Transform parent;
    public Bullet prefab;
    public float speed;
    public float damage;
    [Tooltip("Infinite if <= 0")]
    public float lifeTime;
    public bool destroyOutScreen;
}

public class BulletFountain : MonoBehaviour {

    public GenericSoundsEnum volleySound;
    [SerializeField] private BulletPattern pattern;
    [SerializeField] private Transform origin;
    [SerializeField] private BulletStats bulletStats;

    private IEnumerator routine = null;
    private Transform playerPos;

    public void Init(Transform _playerPos, Transform bulletParent) {
        playerPos = _playerPos;
        bulletStats.parent = bulletParent;
    }

    public void SetFiring(bool fire) {
        if(fire) {
            routine = FireRoutine(); // check if I can do that only at Init
            StartCoroutine(routine);
        }
        else if(routine != null) {
            StopCoroutine(routine);
        }
    }

    IEnumerator FireRoutine() {
        float volleyTimer = pattern.startDelay;

        while (true) {
            if (volleyTimer <= 0) {
                float angleOffset = pattern.angleStart;
                float angleIncrease = pattern.angleBetweenBullets;

                EventDispatcher.DispatchEvent(Events.BULLET_VOLLEY_FIRED, this);

                for (int i = 0; i < pattern.bulletsNb; i++) {
                    Vector3 dir = pattern.targetPlayer ? playerPos.position - origin.position : origin.up;
                    dir = Quaternion.Euler(0, 0, angleOffset) * dir;

                    BulletsFactory.SpawnBullet(bulletStats, origin.position, dir.normalized);

                    if (pattern.delayBetweenBullets > 0) {
                        float timer = pattern.delayBetweenBullets;
                        while (timer >= 0) {
                            yield return null;
                            timer -= Time.deltaTime;
                        }
                    }

                    if (angleOffset > pattern.angleMax || angleOffset < pattern.angleMin) {
                        angleIncrease *= pattern.angleMultiplierAtMax;
                    }

                    angleOffset += angleIncrease;
                }

                volleyTimer = pattern.volleyInterval;
            }

            volleyTimer -= Time.deltaTime;
            yield return null;
        }
    }
}
