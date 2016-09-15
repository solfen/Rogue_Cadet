using UnityEngine;
using System.Collections;

public class TurnPlayer : MonoBehaviour {

    private Transform _transform;

    void Start() {
        _transform = GetComponent<Transform>();
    }
}
