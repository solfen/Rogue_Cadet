using UnityEngine;
using System.Collections;

public class BossRoom : MonoBehaviour {

    [SerializeField] private int bossIndex;
    [SerializeField] private GameObject notBeatenRoom;
    [SerializeField] private GameObject beatenRoom;

    // Use this for initialization
    void Start () {
        bool isBeaten = GlobalData.instance.saveData.bossesBeaten.Contains(bossIndex);
        notBeatenRoom.SetActive(!isBeaten);
        beatenRoom.SetActive(isBeaten);
    }
}
