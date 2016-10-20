using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
    public int x;
    public int y;
    public float pathCost = -1;
    public float totalCost = -1;
    public Node parent;
    public MapEnum type;
    public bool closed;
    public DebugNodePath node;
}

public enum MapEnum {
    FLOOR,
    WALL
}

public class PathFinding : MonoBehaviour {

    public DebugNodePath nodePrefab;
    public float wallProbability;
    public Vector2 startPos;
    public Vector2 endPos;
    public float timeTaken;

    private Node[,] nodes = new Node[10, 10]; //TODO create the map
    private LinkedList<Node> openNodes = new LinkedList<Node>();
    private LinkedList<Node> closedNodes = new LinkedList<Node>();
    private Node targetNode;
    private Node startNode;

    void Start() {
        for(int i = 0; i < 10; i++) {
            for(int j = 0; j < 10; j++) {
                DebugNodePath node = Instantiate(nodePrefab, new Vector3(i * 5, j * 5, 0), Quaternion.identity) as DebugNodePath;
                Node realNode = new Node();

               // node.cellNb.text = "x: " + i + " y: " + j;
                realNode.node = node;
                realNode.x = i;
                realNode.y = j;
                realNode.type = Random.Range(0, 100) < wallProbability ? MapEnum.WALL : MapEnum.FLOOR;
                node.sprite.color = realNode.type == MapEnum.WALL ? Color.blue : Color.white;

                nodes[i, j] = realNode;
            }
        }
        float time = Time.realtimeSinceStartup;
        FindPath(new Vector2(0, 0), new Vector2(9, 9));
        Debug.Log((Time.realtimeSinceStartup - time) * 1000);
    }

    public void FindPath(Vector2 pos, Vector2 targetPos) {
        targetNode = nodes[(int)targetPos.x, (int)targetPos.y];
        startNode = nodes[(int)pos.x, (int)pos.y];
        openNodes.AddLast(startNode);


        while(openNodes.Count > 0) {
            LinkedListNode<Node> lowestNode = openNodes.First;
            float lowestCost = lowestNode.Value.totalCost;

            for (LinkedListNode<Node> nodeListItem = lowestNode.Next; nodeListItem != null; nodeListItem = nodeListItem.Next) {
                Node node = nodeListItem.Value;
                if (node.totalCost <= lowestCost) {
                    lowestCost = node.totalCost;
                    lowestNode = nodeListItem;
                }
            }

            openNodes.Remove(lowestNode);
            closedNodes.AddLast(lowestNode);

            Node currentNode = lowestNode.Value;
            currentNode.closed = true;
            //currentNode.node.sprite.color = Color.green;
            if (targetNode == currentNode) {
                CalculatePath();
                return; //yield break;
            }

            for(int i = currentNode.x-1; i <= currentNode.x+1; i++) {
                for (int j = currentNode.y - 1; j <= currentNode.y + 1; j++) {
                    if (i >= 0 && i < 10 && j >= 0 && j < 10) {
                        Node adjacentNode = nodes[i, j];

                        if (adjacentNode.type != MapEnum.WALL && !adjacentNode.closed) {
                            if(adjacentNode.pathCost == -1) {
                                NodeCost(adjacentNode, currentNode);
                                openNodes.AddLast(adjacentNode);
                                //adjacentNode.node.sprite.color = Color.yellow;
                            }
                            else if(currentNode.pathCost + 1 < adjacentNode.pathCost) {
                                NodeCost(adjacentNode, currentNode);
                            }
                        }
                    }
                }
            }

           /* while (!Input.GetKeyDown(KeyCode.A)) {
                yield return null;
            }

            yield return null;*/
        }


    }

    private void NodeCost(Node adjacentNode, Node node) {
        adjacentNode.pathCost = node.pathCost + 1;
        adjacentNode.totalCost = adjacentNode.pathCost + Mathf.Abs(adjacentNode.x - targetNode.x) + Mathf.Abs(adjacentNode.y - targetNode.y);
        adjacentNode.parent = node;

       // adjacentNode.node.GText.text = "G: " + adjacentNode.pathCost;
        //adjacentNode.node.HText.text = "H: " + Mathf.Abs(adjacentNode.x - targetNode.x) + Mathf.Abs(adjacentNode.y - targetNode.y);
        //adjacentNode.node.EText.text = "E: " + adjacentNode.totalCost;
    }

    private List<Node> CalculatePath() {
        Node currentNode = closedNodes.Last.Value;
        List<Node> path = new List<Node>();

        while(currentNode.parent != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
            //currentNode.node.sprite.color = Color.red;
        }

        return path;
    }
}
