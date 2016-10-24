using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ColliderInfo {
    public float x;
    public float y;
    public float width;
    public float height;
}

/*[Tiled2Unity.CustomTiledImporter]
class CustomImporterAddComponent : Tiled2Unity.ICustomTiledImporter {
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props) {
        // Simply add a component to our GameObject
        if (props.ContainsKey("AddComp")) {
            gameObject.AddComponent(props["AddComp"]);
        }
    }


    public void CustomizePrefab(GameObject prefab) {
        // Do nothing
    }
}*/

[Tiled2Unity.CustomTiledImporterAttribute]
public class RoomCollidersGeneratorTool : Tiled2Unity.ICustomTiledImporter {

    private List<ColliderInfo> colliders = new List<ColliderInfo>();

    public void CustomizePrefab(GameObject prefab) { }

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

}
