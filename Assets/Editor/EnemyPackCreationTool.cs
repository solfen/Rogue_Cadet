using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class EnemyPackCreationTool : MonoBehaviour {

    public Room associatedRoom;
    public List<GameObject> enemiesPrefab;

    public GameObject enemyPack;
    public string packName;
    public float packProbaMultiplier = 1;
    public int zone;

    private Dictionary<string, GameObject> prefabDictionary;

	// Update is called once per frame
	void Update () {
        if (EditorUtility.DisplayDialog("Add new pack ?", "You sure? Have you check all the variables?", "Yeah, yeah shut up.", "I'm so stupid! Thanks!")) {

            prefabDictionary = new Dictionary<string, GameObject>();
            for (int i = 0; i < enemiesPrefab.Count; i++) {
                prefabDictionary.Add(enemiesPrefab[i].name, enemiesPrefab[i]);
            }

            EnemyPack pack = new EnemyPack();
            pack.probabilityMultiplier = packProbaMultiplier;
            pack.zone = zone;
            pack.name = packName;

            for (int i = 0; i < enemyPack.transform.childCount; i++) {
                EnemyInstantiation enemy = new EnemyInstantiation();
                Transform obj = enemyPack.transform.GetChild(i);
                enemy.Enemy = prefabDictionary[obj.name.Substring(0, 6)]; // will not work when there's more than 9 enemiesTypes
                enemy.position = obj.localPosition;
                enemy.rotation = obj.localRotation;
                pack.enemies.Add(enemy);
            }

            associatedRoom.possibleEnemies.Add(pack);
            EditorUtility.SetDirty(associatedRoom);
        }

        enabled = false;
	}
}
