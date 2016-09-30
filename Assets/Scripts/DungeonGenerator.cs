using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomList { //rooms that have the same exits and size
    public List<Room> rooms = new List<Room>();
}

[System.Serializable]
public class ZoneRooms {
    public string name;
    public int zoneIndex;
    public List<RoomList> roomList;
}

[System.Serializable]
public class BossRoom {
    public List<Room> roomsBasePrefab;
    public List<Room> roomsBoss;
    [HideInInspector]
    public bool canPop = true;
}

public class GraphRoom {
    public Vector2 pos = new Vector2();
    public List<GraphRoom> roomsConnected;
    public Room roomPrefab { get; set; }
    public Room roomInstance;
}

public class DungeonGenerator : MonoBehaviour {

    public List<Transform> roomsParent;
    public Vector2 startingPos;
    public Room startingRoom;
    public List<ZoneRooms> zoneRooms; 
    public List<ZoneRooms> deadEndRooms;
    public List<BossRoom> bossRooms;
    public int minRoomsBeforeDeadEnd = 10;
    public float secondsBetweenInstanciation = 2;

    private World world;
    private GraphRoom currentRoom;
    private Exit currentExit;
    private List<GraphRoom> graph;
    private List<GraphRoom> firstDepthGrag;
    private int currentZoneIndex;
    private int currentRoomsNb;
    private Vector2 nextRoomPos = Vector3.zero;
    private Vector3 roomWorldPos = new Vector3();
    private bool deadEndAdded;
    private List<RoomList> roomList;
    private IEnumerable<int> roomListIndexes;

    // Use this for initialization
    void Start() {
        world = World.instance;

        float time = Time.realtimeSinceStartup;
        CreateRoomGraph();
        Debug.Log((Time.realtimeSinceStartup - time)*1000);

        MiniMap.instance.OnGraphCreated(graph);
        StartCoroutine(InstantiateRooms());
    }

    private void CreateRoomGraph() {
        world.InitMap();

        for (int i = 0; i < bossRooms.Count; i++) {
            bossRooms[i].canPop = PlayerPrefs.GetInt("Defeated_Boss" + i) == 0;
        }

        graph = new List<GraphRoom>((int)(world.worldSize.x * world.worldSize.y));
        firstDepthGrag = new List<GraphRoom>((int)(world.worldSize.x * world.worldSize.y));
        currentZoneIndex = 0;
        currentRoomsNb = 0;
        deadEndAdded = false;

        CreateRoom(startingPos, startingRoom);
        currentExit = currentRoom.roomPrefab.exits[0]; //assumes that the starting room only has one exit

        GenerateGraph();

       if(!TryAddBosses()) {
            CreateRoomGraph();
        }
    }

    private void GenerateGraph() {
        do {
           if(!deadEndAdded && currentRoomsNb >= minRoomsBeforeDeadEnd) {
                deadEndAdded = true;
                zoneRooms.AddRange(deadEndRooms);
            }

            GetNextZone((int)(currentRoom.pos.x + currentExit.pos.x), (int)(currentRoom.pos.y + currentExit.pos.y));
            GenerateRoom();

        } while (firstDepthGrag.Count != 1);
    }
    
    IEnumerator InstantiateRooms() {
        for (int i = 0; i < graph.Count; i++) {
            roomWorldPos.Set(graph[i].pos.x * world.roomBaseSize.x, graph[i].pos.y * world.roomBaseSize.y, 0);
            graph[i].roomInstance = Instantiate(graph[i].roomPrefab, roomWorldPos, Quaternion.identity, roomsParent[world.map[(int)graph[i].pos.x, (int)graph[i].pos.y].zoneType]) as Room;
            graph[i].roomInstance.gameObject.SetActive(false);

            if (secondsBetweenInstanciation > 0) {
                yield return new WaitForSeconds(secondsBetweenInstanciation);
            }
        }
    }
    private void GenerateRoom() {
        List<int> roomsToCheck = new List<int>();
        roomsToCheck.AddRange(roomListIndexes); 
        while(roomsToCheck.Count > 0) {
            int i = roomsToCheck[Random.Range(0, roomsToCheck.Count)];
            Room randomRoom = roomList[i].rooms[Random.Range(0, roomList[i].rooms.Count)]; //misleading name. it's a random configuration of the same Room type
            for (int j = 0; j < randomRoom.exits.Count; j++) {
                if (randomRoom.exits[j].dir * -1 == currentExit.dir) {
                    // ok it's complicated. I'm pretty sure there's an easier way to do that.
                    // basicly it set the new pos of the room. Since the previous room exit gives the possition of the next room it should be easy right? WRONG
                    // because I have to take into acount the offset of the chosen exit of the new room
                    // and tey don't have the same refenrenctiel at all!
                    // so I calculated the place the current room exit should be (by reversing the dir of the previous room)
                    // then I calculated where the exit will actually be (exit pos of the previous room + exit pos of the current room)
                    // now I can do a simple substraction and get the offset!
                    // man I really suck at those things...

                    nextRoomPos.x = currentRoom.pos.x + currentExit.pos.x - ((currentExit.pos.x + randomRoom.exits[j].pos.x) - (currentExit.pos.x - 1 * currentExit.dir.x)); 
                    nextRoomPos.y = currentRoom.pos.y + currentExit.pos.y - ((currentExit.pos.y + randomRoom.exits[j].pos.y) - (currentExit.pos.y - 1 * currentExit.dir.y));

                   if(RoomHasPlace(nextRoomPos, randomRoom.size)) {
                        CreateRoom(nextRoomPos, randomRoom);

                        currentRoom.roomsConnected.Add(firstDepthGrag[firstDepthGrag.Count - 2]);
                        firstDepthGrag[firstDepthGrag.Count - 2].roomsConnected.Add(currentRoom);

                        GetUnconnectedExit();
                        return;
                    }
                }
            }

            roomsToCheck.Remove(i);
        }

        world.map[(int)(currentRoom.pos.x + currentExit.pos.x), (int)(currentRoom.pos.y + currentExit.pos.y)].notBuildableFlag = true;
        GetUnconnectedExit();
    }

    private void CreateRoom(Vector2 pos, Room roomPrefab) {
        currentRoom = new GraphRoom();
        currentRoom.pos.Set(pos.x, pos.y);
        currentRoom.roomsConnected = new List<GraphRoom>(roomPrefab.exits.Count);
        currentRoom.roomPrefab = roomPrefab;
        graph.Add(currentRoom);
        firstDepthGrag.Add(currentRoom);
        MarkMapWithRoom(pos, roomPrefab.size);
        currentRoomsNb++;
    }

    private bool TryAddBosses() {
        for (int i = bossRooms.Count-1; i >= 0; i--) {
            if (bossRooms[i].canPop) {
                if(TrySetBossRoom(i)) {
                    bossRooms[i].canPop = false;
                }
                else {
                    return false;
                }
            }
        }

        return true;
    }

    private bool TrySetBossRoom(int i) {
        for(int k = 0; k < bossRooms[i].roomsBasePrefab.Count; k++) {
            for (int j = graph.Count-1; j >= 0 ; j--) {
                if (graph[j].roomPrefab == bossRooms[i].roomsBasePrefab[k]) {
                    graph[j].roomPrefab = bossRooms[i].roomsBoss[k];
                    return true;
                }
            }
        }

        return false;
    }

    private void GetNextZone(int x, int y) {
        if (x >= 0 && x < world.worldSize.x && y >= 0 && y < world.worldSize.y) {
            currentZoneIndex = world.map[x, y].zoneType;
            roomList = zoneRooms[currentZoneIndex].roomList;
            roomListIndexes = System.Linq.Enumerable.Range(0, roomList.Count); // so that we have a list like [0,1,2,3 ... count-1]
        }
    }

    private void GetUnconnectedExit() {

        List<int> exitsTocheck = new List<int>();
        exitsTocheck.AddRange(System.Linq.Enumerable.Range(0, currentRoom.roomPrefab.exits.Count));
        while (exitsTocheck.Count > 0) {
            int selectedIndex = exitsTocheck[Random.Range(0, exitsTocheck.Count)];
            currentExit = currentRoom.roomPrefab.exits[selectedIndex];
            int x = (int)(currentRoom.pos.x + currentExit.pos.x);
            int y = (int)(currentRoom.pos.y + currentExit.pos.y);

            if(x >= 0 && x < world.worldSize.x && y >= 0 && y < world.worldSize.y && !world.map[x, y].notBuildableFlag) {
                GraphRoom nextRoom = world.map[x, y].room;
                if (nextRoom == null) {
                    return;
                }
                else if(!currentRoom.roomsConnected.Contains(nextRoom) || !nextRoom.roomsConnected.Contains(currentRoom)) {
                    int wantedExitPosX = x + (int)currentExit.dir.x * - 1;
                    int wantedExitPosY = y + (int)currentExit.dir.y * - 1;
                    
                    for(int i = 0; i < nextRoom.roomPrefab.exits.Count; i++) {
                        int exitX = (int)(nextRoom.pos.x + nextRoom.roomPrefab.exits[i].pos.x);
                        int exitY = (int)(nextRoom.pos.y + nextRoom.roomPrefab.exits[i].pos.y);

                        if(wantedExitPosX == exitX && wantedExitPosY == exitY) {
                            currentRoom.roomsConnected.Add(nextRoom);
                            nextRoom.roomsConnected.Add(currentRoom);
                        }
                    }
                }
            }

            exitsTocheck.Remove(selectedIndex);
        }

        ReturnToPreviousRoom();
        if(firstDepthGrag.Count > 1)
            GetUnconnectedExit();
    }

    private void ReturnToPreviousRoom() {
        firstDepthGrag.Remove(currentRoom);
        if (firstDepthGrag.Count > 1) {
            currentRoom = firstDepthGrag[firstDepthGrag.Count - 1];
        }
    }

    private bool RoomHasPlace(Vector2 posStart, Vector2 size) {
        Vector2 posEnd = posStart + size;
        for (int x = (int)posStart.x; x < posEnd.x; x++) {
            for (int y = (int)posStart.y; y < posEnd.y; y++) {
                if(x < 0 || x >= world.worldSize.x
                || y < 0 || y >= world.worldSize.y
                || world.map[x, y].room != null) {
                    return false;
                }
            }
        }

        return true;
    }

    private void MarkMapWithRoom(Vector2 posStart, Vector2 size) {
        Vector2 posEnd = posStart + size;
        for (int x = (int)posStart.x; x < posEnd.x; x++) {
            for (int y = (int)posStart.y; y < posEnd.y; y++) {
                world.map[x, y].room = currentRoom;
            }
        }
    }

}
