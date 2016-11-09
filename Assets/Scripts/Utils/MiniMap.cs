using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {
    public static MiniMap instance;

    public GameData gameData;
    public Dungeon dungeon;
    public RectTransform roomPrefab;
    public RectTransform exitPrefab;
    public Transform roomsParent;
    public float scale;
    public Vector2 smallMapSize;

    private Dictionary<GraphRoom, GameObject> rooms = new Dictionary<GraphRoom, GameObject>();
    private RectTransform _rectTransform;
    private RectTransform currentRoom;
    private RectTransform currentExit;
    private Vector2 roomBaseSize = new Vector2();
    private Vector3 currentPos = new Vector3();
    private Vector3 currentExitPos = new Vector3(); 
    private Vector2 currentSize = new Vector2();
    private Vector2 currentExitSize = new Vector2();
    private Vector2 bigMapSize;
    private Vector2 currentMapSize;
    private Transform playerTransform;
    private RectTransform roomparentTransform;

    // Use this for initialization
    void Awake () {
        instance = this;
        EventDispatcher.AddEventListener(Events.DUNGEON_GRAPH_CREATED, OnGraphCreated);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.DUNGEON_GRAPH_CREATED, OnGraphCreated);
    }

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        roomparentTransform = roomsParent.GetComponent<RectTransform>();
        _rectTransform = GetComponent<RectTransform>();

        roomBaseSize = gameData.roomBaseSize * scale;

        smallMapSize.Set(smallMapSize.x * roomBaseSize.x, smallMapSize.y * roomBaseSize.y);
        bigMapSize = new Vector2(gameData.worldSize.x * roomBaseSize.x, gameData.worldSize.y * roomBaseSize.y);
        _rectTransform.sizeDelta = smallMapSize;
        currentMapSize = smallMapSize;
    }

    void Update() {
        if (playerTransform != null) {
            roomparentTransform.anchoredPosition = -playerTransform.position * scale;

            if (Input.GetButtonDown("Map")) {
                currentMapSize = currentMapSize == smallMapSize ? bigMapSize : smallMapSize;
                _rectTransform.sizeDelta = currentMapSize;
            }
        }

    }

    public void OnGraphCreated(object graphObject) {
        List<GraphRoom> graph = (List<GraphRoom>)graphObject;

        for (int i = 0; i < graph.Count; i++) {
            currentPos.Set(graph[i].pos.x * roomBaseSize.x, graph[i].pos.y * roomBaseSize.y, 0);
            currentSize.Set(roomBaseSize.x * graph[i].roomPrefab.size.x, roomBaseSize.y * graph[i].roomPrefab.size.y);

            currentRoom = Instantiate(roomPrefab, roomsParent) as RectTransform;
            currentRoom.localPosition = currentPos;
            currentRoom.sizeDelta = currentSize;

            rooms.Add(graph[i], currentRoom.gameObject);

            for (int j = 0; j < graph[i].roomPrefab.exits.Count; j++) {
                int x = (int)(graph[i].pos.x + graph[i].roomPrefab.exits[j].pos.x);
                int y = (int)(graph[i].pos.y + graph[i].roomPrefab.exits[j].pos.y);

                if (x >= 0 && x < gameData.worldSize.x && y >= 0 && y < gameData.worldSize.y && graph[i].roomsConnected.Contains(dungeon.map[x,y].room)) {
                    currentExitPos.Set(Mathf.Max(0,graph[i].roomPrefab.exits[j].pos.x) * roomBaseSize.x + 0.5f * roomBaseSize.x * Mathf.Abs(graph[i].roomPrefab.exits[j].dir.y), Mathf.Max(0, graph[i].roomPrefab.exits[j].pos.y) * roomBaseSize.y + 0.5f * roomBaseSize.y * Mathf.Abs(graph[i].roomPrefab.exits[j].dir.x), 0);
                    currentExitSize.Set(roomBaseSize.y * (0.10f + 0.38f * Mathf.Abs(graph[i].roomPrefab.exits[j].dir.y)), roomBaseSize.y * (0.10f + 0.38f * Mathf.Abs(graph[i].roomPrefab.exits[j].dir.x)));

                    currentExit = Instantiate(exitPrefab, currentRoom.transform) as RectTransform;
                    currentExit.localPosition = currentExitPos;
                    currentExit.sizeDelta = currentExitSize;
                }
            }

           currentRoom.gameObject.SetActive(false);
        }
    }

    public void OnPlayerEnterRoom(GraphRoom room) {
        rooms[room].SetActive(true);
    }
}
