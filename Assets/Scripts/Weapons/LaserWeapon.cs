using UnityEngine;
using System.Collections;
using System;

public class LaserWeapon : BaseWeapon {

    [SerializeField] private float maxLength;
    [SerializeField] private float baseDamage;
    //[SerializeField] private float fireDuration;
    [SerializeField] private LayerMask laserMask;
    [SerializeField] private GameObject laserSpriteStart;
    [SerializeField] private GameObject laserSpriteMiddle;
    [SerializeField] private GameObject laserSpriteEnd;

    private Transform _transform;
    private Transform laserStart;
    private Transform laserMiddle;
    private Transform laserEnd;

    private Vector3 laserMiddleScale = new Vector3();
    private Vector3 laserEndPos = new Vector3();

    protected override void Start () {
        base.Start();

        _transform = GetComponent<Transform>();

        laserStart = (Instantiate(laserSpriteStart, _transform, false) as GameObject).GetComponent<Transform>();
        laserMiddle = (Instantiate(laserSpriteMiddle, _transform, false) as GameObject).GetComponent<Transform>();
        laserEnd = (Instantiate(laserSpriteEnd, _transform, false) as GameObject).GetComponent<Transform>();
        SetLaserActive(false);

        laserEnd.GetComponent<DamageDealer>().damage = baseDamage * damageInfluencer;
    }

    // Update is called once per frame
    IEnumerator LaserLength() {
        while(true) {
            float laserLength = maxLength;

            RaycastHit2D hit = Physics2D.Raycast(_transform.position, _transform.right, maxLength, laserMask.value);
            if (hit.collider != null) {
                laserLength = Vector2.Distance(hit.point, _transform.position);
            }

            //each laser sprite is 1 unit long. So -2 is for length - start - end
            laserMiddleScale.Set(laserLength - 2, laserMiddle.localScale.y, laserMiddle.localScale.z);
            laserEndPos.Set(laserLength-1, laserEnd.localPosition.y, laserEnd.localPosition.z);

            laserMiddle.localScale = laserMiddleScale;
            laserEnd.localPosition = laserEndPos;

            fireTimer += Time.deltaTime * 2; //fire timer cools at deltaTime rate, so when the laser is in use it needs to raise the timer by 2 deltaTime.

            yield return null;
        }
    }

    protected override void SetFiring(bool fireState) {
        if(fireState) {
            SetLaserActive(true);
            StartCoroutine("LaserLength");
        }
        else {
            StopCoroutine("LaserLength");
            SetLaserActive(false);
        }
    }

    private void SetLaserActive(bool state) {
        laserStart.gameObject.SetActive(state);
        laserMiddle.gameObject.SetActive(state);
        laserEnd.gameObject.SetActive(state);
    }
}
