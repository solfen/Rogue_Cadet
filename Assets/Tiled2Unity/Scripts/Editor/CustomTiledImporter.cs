using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Tiled2Unity;

public class ColliderInfo {
    public float x;
    public float y;
    public float width;
    public float height;
}

[CustomTiledImporterAttribute]
public class CustomTiledImporter : ICustomTiledImporter {

    private List<Exit> exits = new List<Exit>();
    private float mapWidth;
    private float mapHeight;
    private float topBorderMaxVerticeX = 0;
    private float bottomBorderMaxVerticeX = 0;
    private float leftBorderMinVerticeY = 0;
    private float rightBorderMinVerticeY = 0;
    private int zoneNb = -1;
    private Dictionary<float, List<ColliderInfo>> colliders = new Dictionary<float, List<ColliderInfo>>();
    private List<ColliderInfo> collidersList = new List<ColliderInfo>();

    public void CustomizePrefab(GameObject prefab) {
        Transform existingPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/Rooms/Zone" + zoneNb + "/" + prefab.name + ".prefab");
        GameObject roomGameObject;
        GameObject newTiledObj = Object.Instantiate(prefab, new Vector3(prefab.transform.position.x, mapHeight, prefab.transform.position.z), Quaternion.identity) as GameObject;
        newTiledObj.name = "TiledMap";

        if(zoneNb == -1) {
            Debug.LogError("Room has no \"zoneNb\" Tiled property! Add one!");
        }

        if(existingPrefab != null) {
            roomGameObject = Object.Instantiate(existingPrefab.gameObject);

            Object.DestroyImmediate(roomGameObject.transform.GetChild(0).gameObject); //Destroy old TiledMap

            newTiledObj.transform.parent = roomGameObject.transform;
            newTiledObj.transform.SetAsFirstSibling();

            roomGameObject.GetComponent<Room>().exits = exits;

            PrefabUtility.ReplacePrefab(roomGameObject, existingPrefab, ReplacePrefabOptions.ReplaceNameBased);
        }
        else {
            roomGameObject = new GameObject(prefab.name);
            GameObject enemiesGameObject = new GameObject("Enemies");

            Room roomComponent = roomGameObject.gameObject.AddComponent<Room>();
            roomComponent.zoneIndex = zoneNb;
            roomComponent.size = new Vector2(mapWidth / 60, mapHeight / 34); //hard coded == bad. But It's simplier. BTW future me if you had trouble because of that, I'm sory.
            roomComponent.enemiesParent = enemiesGameObject.transform;
            newTiledObj.transform.parent = roomGameObject.transform;
            enemiesGameObject.transform.parent = roomGameObject.transform;

            roomComponent.exits = exits;

            PrefabUtility.CreatePrefab("Assets/Prefabs/Rooms/Zone" + zoneNb + "/" + prefab.name + ".prefab", roomGameObject);
        }

        Object.DestroyImmediate(roomGameObject);
    }

    public void HandleCustomProperties(GameObject layer, IDictionary<string, string> props) {
        foreach(KeyValuePair<string, string> elem in props) {
            if(elem.Key == "obstaclesLayer") {
                GetColliders(layer);
                FindRoomExits();
            }
            else if(elem.Key == "zoneNb") {
                zoneNb = int.Parse(elem.Value);
            }
        }
    }

    private void GetColliders(GameObject layer) {
        if (layer.GetComponentInChildren<MeshFilter>() == null) {
            Debug.LogError("no obstacle mesh!");
            return;
        }

        Vector3[] vertices = layer.GetComponentInChildren<MeshFilter>().sharedMesh.vertices;
        int[] triangles = layer.GetComponentInChildren<MeshFilter>().sharedMesh.triangles;
        mapWidth = layer.transform.parent.GetComponent<TiledMap>().NumTilesWide;
        mapHeight = layer.transform.parent.GetComponent<TiledMap>().NumTilesHigh;

        for ( int i = 1; i < triangles.Length; i+=3) {
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i+1]];

            if(!colliders.ContainsKey(v1.y)) {
                colliders.Add(v1.y, new List<ColliderInfo>());
            }

            FindAndUpdateColliderBasedOnVertices(v1, v2);

            //Debug.Log(vertices[triangles[i-1]].x + ":" + vertices[triangles[i-1]].y + " , " + v1.x + ":" + v1.y + " , " + v2.x + ":" + v2.y);
        }

        MergeColliders();

        GameObject collidersObj = new GameObject("colliders");
        foreach (KeyValuePair<float, List<ColliderInfo>> elem in colliders) {
            for(int i = 0; i < elem.Value.Count; i++) {
                ColliderInfo colData = elem.Value[i];
                BoxCollider2D col = collidersObj.AddComponent<BoxCollider2D>();
                col.offset = new Vector2(colData.x + colData.width / 2, (colData.y - colData.height / 2));
                col.size = new Vector2(colData.width, colData.height);
                collidersList.Add(colData);
            }
        }

        collidersObj.layer = LayerMask.NameToLayer("Walls");
        collidersObj.tag = "Wall";
        collidersObj.transform.parent = layer.transform;
    }

    private void FindAndUpdateColliderBasedOnVertices(Vector3 v1, Vector3 v2) {
        for (int i = 0; i < colliders[v1.y].Count; i++) {
            ColliderInfo col = colliders[v1.y][i];
            if (v1.x == col.x + col.width) {
                col.width += (v2.x - v1.x);
                return;
            }
            else if (v2.x == col.x) {
                col.width += (col.x - v1.x);
                col.x = v1.x;
                return;
            }
            else if(v2.x > col.x && v1.x < col.x + col.width) {
                return;
            }
        }

        ColliderInfo newCol = new ColliderInfo();
        newCol.x = v1.x;
        newCol.y = v1.y;
        newCol.width = v2.x - v1.x;
        newCol.height = 1;

        colliders[v1.y].Add(newCol);
    }

    private void MergeColliders() {
        var enumerator = colliders.Values.GetEnumerator();
        enumerator.MoveNext();
        List<ColliderInfo> previousRow = enumerator.Current;
        MergeX(previousRow);

        while (enumerator.MoveNext()) {
            MergeX(enumerator.Current);
            MergeY(enumerator.Current, previousRow);
            previousRow = enumerator.Current;
        }
    }

    private void MergeX(List<ColliderInfo> colliders) {
        colliders.Sort((a, b) => (int)(a.x - b.x));
        for (int i = colliders.Count - 1; i > 0; i--) {
            if (colliders[i].x == colliders[i - 1].x + colliders[i - 1].width) {
                colliders[i - 1].width += colliders[i].width;
                colliders.RemoveAt(i);
            }
        }
    }

    private void MergeY(List<ColliderInfo> row, List<ColliderInfo> upRow) {
        for (int i = row.Count - 1; i >= 0; i--) {
            for (int j = upRow.Count - 1; j >= 0; j--) {
                if (row[i].x == upRow[j].x && row[i].width == upRow[j].width && upRow[j].y - upRow[j].height == row[i].y) {
                    row[i].y = upRow[j].y;
                    row[i].height += upRow[j].height;
                    upRow.RemoveAt(j);
                    break;
                }
            }
        }
    }

    private void CreateExitFromPos(float verticeX, float verticeY, bool isHorizontal) {
        Exit exit = new Exit();
        exit.pos = new Vector2(Mathf.Ceil(verticeX / 60) - 1, Mathf.Ceil((mapHeight + verticeY) / 34) - 1);
        exit.dir = isHorizontal ? new Vector2(0, verticeY == -mapHeight ? -1 : 1) : new Vector2(verticeX == 0 ? -1 : 1, 0);

        exits.Add(exit);
    }

    //this is ugly, I'm pretty sure it could be factorized. But hey, that way it's dead simple
    private void FindRoomExits() {

        for(int i = 0; i < collidersList.Count; i++) {
            for(int j = 0; j < collidersList.Count; j++) {
                ColliderInfo col1 = collidersList[i], col2 = collidersList[j];
                if (col1.y == 0 && col2.y == 0 && col1.x + col1.width == col2.x-6) {
                    CreateExitFromPos(col2.x, 1, true);
                }
                else if(col1.y - col1.height == -mapHeight && col2.y - col2.height == -mapHeight && col1.x + col1.width == col2.x - 6) {
                    CreateExitFromPos(col2.x, -mapHeight, true);
                }

                if (col1.x == 0 && col2.x == 0 && col1.y - col1.height == col2.y + 6) {
                    CreateExitFromPos(0, col2.y, false);
                }
                else if(col1.x + col1.width == mapWidth && col2.x + col2.width == mapWidth && col1.y - col1.height == col2.y + 6) {
                    CreateExitFromPos(mapWidth+1, col2.y, false);
                }
            } 
        }
    }
}
