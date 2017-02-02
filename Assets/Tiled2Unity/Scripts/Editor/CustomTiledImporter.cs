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

    private bool CheckColliderPos(int i, float tileWidth, float tileHeight) {
        return colliders[i].x > 0 && colliders[i].x + colliders[i].width < tileWidth && colliders[i].y > 0 && colliders[i].y < tileHeight;
    }

    public void CustomizePrefab(GameObject prefab) {
        Transform existingPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Prefabs/Rooms/Zone0" + prefab.name + ".prefab"); //!!!!!!!!!!!!!!!!!!! TODO: Tiled custom property to set zone !!!!!!!!!!!!!!!!!

        GameObject roomGameObject;
        GameObject newTiledObj = Object.Instantiate(prefab, new Vector3(prefab.transform.position.x, mapHeight, prefab.transform.position.z), Quaternion.identity) as GameObject;
        newTiledObj.name = "TiledMap";

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
            roomComponent.size = new Vector2(mapWidth / 60, mapHeight / 34); //hard coded == bad. But It's simplier. BTW future me if you had trouble because of that, I'm sory.
            roomComponent.enemiesParent = enemiesGameObject.transform;

            newTiledObj.transform.parent = roomGameObject.transform;
            enemiesGameObject.transform.parent = roomGameObject.transform;

            roomComponent.exits = exits;

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

        mapWidth = layer.transform.parent.GetComponent<TiledMap>().NumTilesWide;
        mapHeight = layer.transform.parent.GetComponent<TiledMap>().NumTilesHigh;

        for (int i = 1; i < vertices.Length; i++) {
            FindRoomExit(vertices[i], previousX);

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

    private void CreateExitFromPos(float verticeX, float verticeY, bool isHorizontal) {
        Debug.Log("x: " + verticeX + "  y: " + verticeY);
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
