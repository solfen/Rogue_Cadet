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

    private List<ColliderInfo> colliders = new List<ColliderInfo>();
    private List<Exit> exits = new List<Exit>();
    private float mapWidth;
    private float mapHeight;
    private float leftBorderMinVerticeY = 0;
    private float rightBorderMinVerticeY = 0;
    private int zoneNb = -1;
    private Dictionary<float, List<ColliderInfo>> collidersNew = new Dictionary<float, List<ColliderInfo>>();

    private bool CheckColliderPos(int i, float tileWidth, float tileHeight) {
        return colliders[i].x > 0 && colliders[i].x + colliders[i].width < tileWidth && colliders[i].y > 0 && colliders[i].y < tileHeight;
    }

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
                GetCollidersAndExit(layer);
            }
            else if(elem.Key == "zoneNb") {
                zoneNb = int.Parse(elem.Value);
            }
        }
    }

    private void FindAndUpdateColliderBasedOnVertices(Vector3 v1, Vector3 v2) {
        for (int i = 0; i < collidersNew[v1.y].Count; i++) {
            ColliderInfo col = collidersNew[v1.y][i];
            if (v1.x == col.x + col.width) {
                col.width += (v2.x - v1.x);
                return;
            }
            else if(v2.x == col.x) {
                col.width += (col.x - v1.x);
                col.x = v1.x;
                return;
            }
        }

        ColliderInfo newCol = new ColliderInfo();
        newCol.x = v1.x;
        newCol.y = v1.y;
        newCol.width = v2.x - v1.x;
        newCol.height = 1;

        collidersNew[v1.y].Add(newCol);
    }

    private void MergeColliders() {
        var enumerator = collidersNew.Values.GetEnumerator();
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
        for(int i = colliders.Count - 1; i > 0; i--) {
            if(colliders[i].x == colliders[i-1].x + colliders[i-1].width) {
                colliders[i - 1].width += colliders[i].width;
                colliders.RemoveAt(i);
            }
        }
    }

    private void MergeY(List<ColliderInfo> row, List<ColliderInfo> upRow) {
        for (int i = row.Count - 1; i >= 0; i--) {
            for(int j = upRow.Count - 1; j >= 0; j--) {
                if (row[i].x == upRow[j].x && row[i].width == upRow[j].width) {
                    row[i].y = upRow[j].y;
                    row[i].height += upRow[j].height;
                    upRow.RemoveAt(j);
                    break;
                }
            }
        }
    }

    private void GetCollidersAndExit(GameObject layer) {
        if (layer.GetComponentInChildren<MeshFilter>() == null) {
            Debug.LogError("no obstacle mesh!");
            return;
        }

        Vector3[] vertices = layer.GetComponentInChildren<MeshFilter>().sharedMesh.vertices;
        int[] triangles = layer.GetComponentInChildren<MeshFilter>().sharedMesh.triangles;
        Debug.Log(layer.GetComponentInChildren<MeshFilter>().sharedMesh.subMeshCount);
        for( int i = 1; i < triangles.Length; i+=3) {
            Vector3 v1 = vertices[triangles[i]];
            Vector3 v2 = vertices[triangles[i+1]];

            if(!collidersNew.ContainsKey(v1.y)) {
                collidersNew.Add(v1.y, new List<ColliderInfo>());
            }

            FindAndUpdateColliderBasedOnVertices(v1, v2);

            //Debug.Log(vertices[triangles[i-1]].x + ":" + vertices[triangles[i-1]].y + " , " + v1.x + ":" + v1.y + " , " + v2.x + ":" + v2.y);
        }

        MergeColliders();

        GameObject collidersObj = new GameObject("colliders");
        foreach (KeyValuePair<float, List<ColliderInfo>> elem in collidersNew) {
            for(int i = 0; i < elem.Value.Count; i++) {
                ColliderInfo colData = elem.Value[i];
                BoxCollider2D col = collidersObj.AddComponent<BoxCollider2D>();
                col.offset = new Vector2(colData.x + colData.width / 2, (colData.y - colData.height / 2));
                col.size = new Vector2(colData.width, colData.height);
            }
        }

        collidersObj.layer = LayerMask.NameToLayer("Walls");
        collidersObj.tag = "Wall";
        collidersObj.transform.parent = layer.transform;
    }

    private void AddColliderLine(float x, float width, float y) {
        ColliderInfo currentCollider = colliders.Find(collider => collider.x == x && collider.width == width && collider.height == y - 1 - collider.y);
        if (currentCollider == null) {
            currentCollider = new ColliderInfo();
            currentCollider.x = x;
            currentCollider.width = width;
            currentCollider.y = y - 1;
            colliders.Add(currentCollider);
        }

        currentCollider.height = y - currentCollider.y;
    }

    private void CreateExitFromPos(float verticeX, float verticeY, bool isHorizontal) {
        Exit exit = new Exit();
        exit.pos = new Vector2(Mathf.Ceil(verticeX / 60) - 1, Mathf.Ceil((mapHeight + verticeY) / 34) - 1);
        exit.dir = isHorizontal ? new Vector2(0, verticeY == -mapHeight ? -1 : 1) : new Vector2(verticeX == 0 ? -1 : 1, 0);

        exits.Add(exit);
    }

    private void FindRoomExit(Vector3 vertice, float previousX) {
        if (vertice.x == previousX + 6 && (vertice.y == -1 || vertice.y == -mapHeight)) { //horizontal gap
            CreateExitFromPos(vertice.x, vertice.y == -1 ? 1 : vertice.y, true);
        }
        if (vertice.x == 0) { // vertice is on left border
            if (vertice.y == leftBorderMinVerticeY - 7) {
                CreateExitFromPos(vertice.x, vertice.y, false);
            }

            leftBorderMinVerticeY = Mathf.Min(leftBorderMinVerticeY, vertice.y);
        }
        else if (vertice.x == mapWidth) { // vertice is on right border
            if (vertice.y == rightBorderMinVerticeY - 6) {
                CreateExitFromPos(vertice.x+1, vertice.y, false);
            }

            rightBorderMinVerticeY = Mathf.Min(rightBorderMinVerticeY, vertice.y);
        }
    }
}
