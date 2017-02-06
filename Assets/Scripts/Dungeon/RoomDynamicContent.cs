using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Text.RegularExpressions;
using UnityEditor;
[ExecuteInEditMode]
#endif

public class RoomDynamicContent : MonoBehaviour {

    private List<ContentInstance> content;

    #if UNITY_EDITOR
    //take all the childs and transforms it into a list (prefab, pos, rot). Childs are destroyed
    public void ContentToList() {
        content = new List<ContentInstance>();
        Dictionary<string, GameObject> foundPrefabs = new Dictionary<string, GameObject>();
        string regex = "(\\(Clone\\)| \\(\\d+\\))"; //regex to match for (Clone) or  (1)
        string[] where2look = { "Assets/Prefabs/Collectibles", "Assets/Prefabs/Enemies", "Assets/Prefabs/Special" };

        for(int i = transform.childCount-1; i >= 0;  i--) {
            Transform child = transform.GetChild(i);
            string realName = Regex.Replace(child.name, regex, "");
            GameObject prefab;

            if(foundPrefabs.ContainsKey(realName)) {
                prefab = foundPrefabs[realName];
            }
            else {
                string[] matches = AssetDatabase.FindAssets(realName, where2look);
                if(matches.Length == 0) {
                    Debug.LogError("Prefab of: " + realName + " not found! Maybe folder isn't included in search");
                    continue;
                }

                prefab = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(matches[0]), typeof(GameObject)) as GameObject;
                foundPrefabs.Add(realName, prefab);
            }

            ContentInstance contentInstance = new ContentInstance();
            contentInstance.prefab = prefab;
            contentInstance.position = child.localPosition;
            contentInstance.rotation = child.localRotation;

            content.Add(contentInstance);
            DestroyImmediate(child.gameObject);
        }
    }
    #endif

    // Use this for initialization
    void Start() {
        ListToContent();
    }

    //take the content list and make childs based on it
    public void ListToContent() {
        if(content != null) {
            for(int i = content.Count-1; i >= 0; i--) {
                GameObject child = Instantiate(content[i].prefab, transform, false) as GameObject;
                child.transform.localPosition = content[i].position;
                child.transform.localRotation = content[i].rotation;
            }
        }
    }
}

[System.Serializable]
public class ContentInstance {
    public GameObject prefab;
    public Vector3 position;
    public Quaternion rotation;
}
