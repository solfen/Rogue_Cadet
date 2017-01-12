using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Treasure : MonoBehaviour {

    [SerializeField] private List<GameObject> treassures = new List<GameObject>();
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D() {
        GameObject selected = treassures[Random.Range(0, treassures.Count)];
        selected.SetActive(true);
        selected.transform.parent = null;

        GetComponent<Rigidbody2D>().simulated = false;
        anim.SetTrigger("Explode");
        Destroy(gameObject, 0.6f);
    }
}
