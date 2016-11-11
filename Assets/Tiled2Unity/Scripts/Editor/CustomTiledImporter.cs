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

    private bool CheckColliderPos(int i, float tileWidth, float tileHeight) {
        return colliders[i].x > 0 && colliders[i].x + colliders[i].width < tileWidth && colliders[i].y > 0 && colliders[i].y < tileHeight;
    }

    public void CustomizePrefab(GameObject prefab) {
        Transform existingPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/Rooms/" + prefab.name + ".prefab");

        float mapWidth = prefab.GetComponent<TiledMap>().NumTilesWide;
        float mapHeight = prefab.GetComponent<TiledMap>().NumTilesHigh;

        GameObject roomGameObject;
        GameObject newTiledObj = Object.Instantiate(prefab, new Vector3(prefab.transform.position.x, mapHeight, prefab.transform.position.z), Quaternion.identity) as GameObject;
        newTiledObj.name = "TiledMap";

        if(existingPrefab != null) {
            roomGameObject = Object.Instantiate(existingPrefab.gameObject);

            Object.DestroyImmediate(roomGameObject.transform.GetChild(0).gameObject); //Destroy old TiledMap

            newTiledObj.transform.parent = roomGameObject.transform;
            newTiledObj.transform.SetAsFirstSibling();

            FindRoomExits(roomGameObject.GetComponent<Room>(), mapWidth, mapHeight);

            PrefabUtility.ReplacePrefab(roomGameObject, existingPrefab, ReplacePrefabOptions.ReplaceNameBased);
        }
        else {
            roomGameObject = new GameObject(prefab.name);
            GameObject enemiesGameObject = new GameObject("Enemies");

            Room roomComponent = roomGameObject.gameObject.AddComponent<Room>();
            roomComponent.size = new Vector2(mapWidth / 60, mapHeight / 34); //hard coded == bad. But It's simplier. BTW future me if you had trouble because of that, I'm sory.
            roomComponent.enemiesParent = enemiesGameObject.transform;

            newTiledObj.transform.parent = roomGameObject.transform;
            enemiesGameObject.transform.parent = roomGameObject.transform;

            FindRoomExits(roomComponent, mapWidth, mapHeight);

            PrefabUtility.CreatePrefab("Assets/Prefabs/Rooms/" + prefab.name + ".prefab", roomGameObject);
        }

        Object.DestroyImmediate(roomGameObject);
    }

    public void HandleCustomProperties(GameObject layer, IDictionary<string, string> props) {

        if (layer.GetComponentInChildren<MeshFilter>() == null) { // will need to change this if multiple custom importers
            Debug.LogError("no obstacle mesh!");
            return;
        }

        Vector3[] vertices = layer.GetComponentInChildren<MeshFilter>().sharedMesh.vertices;
        float startX = vertices[0].x;
        float previousX = vertices[0].x;
        float previousY = vertices[0].y;
        for (int i = 1; i < vertices.Length; i++) {
            if (vertices[i].x > previousX + 1 || vertices[i].x < previousX) {
                AddColliderLine(startX, previousX - startX, previousY * -1);
                startX = vertices[i].x;
            }

            previousX = vertices[i].x;
            previousY = vertices[i].y;
        }
        AddColliderLine(startX, previousX - startX, previousY * -1);

        GameObject collidersObj = new GameObject("colliders");
        for (int i = 0; i < colliders.Count; i++) {
            BoxCollider2D coll = collidersObj.AddComponent<BoxCollider2D>();
            coll.offset = new Vector2(colliders[i].x + colliders[i].width / 2, -(colliders[i].y + colliders[i].height / 2));
            coll.size = new Vector2(colliders[i].width, colliders[i].height);
            //Debug.Log("x: " + colliders[i].x + " y: " + colliders[i].y + " w: " + colliders[i].width + " h: " + colliders[i].height);
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

    private void FindRoomExits(Room room, float mapWidth, float mapHeight) {
        room.exits.Clear();

        for (int i = 0; i < colliders.Count; i++) {
            if (CheckColliderPos(i, mapWidth, mapHeight)) {
                continue;
            }

            for (int j = 0; j < colliders.Count; j++) {
                if (!CheckColliderPos(j, mapWidth, mapHeight) && i != j) {
                    if (colliders[i].width > colliders[i].height && colliders[j].width > colliders[j].height // same axis check
                    && colliders[i].y == colliders[j].y && colliders[i].x < colliders[j].x) { // pos check
                        Exit exit = new Exit();
                        exit.pos = new Vector2(Mathf.Floor((colliders[i].x + colliders[i].width) / 60), colliders[i].y == 0 ? room.size.y : -1);
                        exit.dir = new Vector2(0, colliders[i].y == 0 ? 1 : -1);
                        room.exits.Add(exit);
                        break;
                    }
                    else if (colliders[i].height > colliders[i].width && colliders[j].height > colliders[j].width
                    && colliders[i].x == colliders[j].x && colliders[i].y < colliders[j].y) {
                        Exit exit = new Exit();
                        exit.pos = new Vector2(colliders[i].x == 0 ? -1 : room.size.x, -Mathf.Floor((colliders[i].y + colliders[i].height) / 34));
                        exit.dir = new Vector2(colliders[i].x == 0 ? -1 : 1, 0);
                        room.exits.Add(exit);
                        break;
                    }
                }
            }
        }
    }
}
