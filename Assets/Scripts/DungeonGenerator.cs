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
    public GameObject boss;
    public Vector2 roomSize;
    public int zoneIndex;
    [HideInInspector]
    public bool canPop = true;
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
    private Room previousRoom;
    private List<Room> graphRooms = new List<Room>();
    private int previousExitIndex;
    private int previousZoneIndex = 0;
    private Vector2 roomPos = Vector3.zero;
    private Vector2 nextRoomPos = Vector3.zero;
    private Vector3 roomWorldPos;
    private int currentRoomsNb;
    private bool deadEndAdded = false;
    private List<RoomList> roomList;

    // Use this for initialization
    void Start () {
        world = World.instance;
        roomPos = startingPos;
        roomWorldPos.Set(roomPos.x * world.roomBaseSize.x, roomPos.y * world.roomBaseSize.y, 0);

        for (int i = 0; i < bossRooms.Count; i++) {
            if (world.isNewGame) {
                PlayerPrefs.SetInt("Defeated_Boss" + i, 0);
            }
            bossRooms[i].canPop = PlayerPrefs.GetInt("Defeated_Boss" + i) == 0;
        }

        previousRoom = Instantiate(startingRoom, roomWorldPos, Quaternion.identity, roomsParent[previousZoneIndex]) as Room;
        graphRooms.Add(previousRoom);
        previousExitIndex = 0; //assumes that the starting room only has one exit
        MarkMapWithRoom(roomPos, roomPos + previousRoom.size);

        StartCoroutine(Generation());
    }

    IEnumerator Generation() {
        //float time = Time.realtimeSinceStartup;
        do {
           if (secondsBetweenInstanciation > 0)
                yield return new WaitForSeconds(secondsBetweenInstanciation);

           if(!deadEndAdded && currentRoomsNb >= minRoomsBeforeDeadEnd) {
                deadEndAdded = true;
                zoneRooms.AddRange(deadEndRooms);
            }

            GetNextZone((int)(roomPos.x + previousRoom.exits[previousExitIndex].pos.x), (int)(roomPos.y + previousRoom.exits[previousExitIndex].pos.y));
            GenerateRoom();

        } while (graphRooms.Count > 1);

        AddBosses();

        yield return null;
        //Debug.Log(Time.realtimeSinceStartup - time);
    }
       

    private void GenerateRoom() {
        List<int> roomsToCheck = new List<int>();
        roomsToCheck.AddRange(System.Linq.Enumerable.Range(0, roomList.Count)); // so that we have a list like [0,1,2,3 ... count-1]
        while(roomsToCheck.Count > 0) {
            int i = roomsToCheck[Random.Range(0, roomsToCheck.Count)];
            Room randomRoom = roomList[i].rooms[Random.Range(0, roomList[i].rooms.Count)];
            for (int j = 0; j < randomRoom.exits.Count; j++) {
                if (randomRoom.exits[j].dir * -1 == previousRoom.exits[previousExitIndex].dir) {
                    // ok it's complicated. I'm pretty sure there's an easier way to do that.
                    // basicly it set the new pos of the room. Since the previous room exit gives the possition of the next room it should be easy right? WRONG
                    // because I have to take into acount the offset of the chosen exit of the new room
                    // and tey don't have the same refenrenctiel at all!
                    // so I calculated the place the current room exit should be (by reversing the dir of the previous room)
                    // then I calculated where the exit will actually be (exit pos of the previous room + exit pos of the current room)
                    // now I can do a simple substraction and get the offset!
                    // man I really suck at those things...

                    nextRoomPos.x = roomPos.x + previousRoom.exits[previousExitIndex].pos.x - ((previousRoom.exits[previousExitIndex].pos.x + randomRoom.exits[j].pos.x) - (previousRoom.exits[previousExitIndex].pos.x - 1 * previousRoom.exits[previousExitIndex].dir.x)); 
                    nextRoomPos.y = roomPos.y + previousRoom.exits[previousExitIndex].pos.y - ((previousRoom.exits[previousExitIndex].pos.y + randomRoom.exits[j].pos.y) - (previousRoom.exits[previousExitIndex].pos.y - 1 * previousRoom.exits[previousExitIndex].dir.y));

                   if(RoomHasPlace(nextRoomPos, nextRoomPos + randomRoom.size)) {
                        previousRoom.exits[previousExitIndex].connected = true;
                        currentRoomsNb++;
                        roomPos.Set(nextRoomPos.x, nextRoomPos.y);
                        MarkMapWithRoom(roomPos, roomPos + randomRoom.size);

                        roomWorldPos.Set(roomPos.x * world.roomBaseSize.x, roomPos.y * world.roomBaseSize.y, 0);
                        previousRoom = Instantiate(randomRoom, roomWorldPos, Quaternion.identity, roomsParent[previousZoneIndex]) as Room;
                        previousRoom.pos.Set(roomPos.x, roomPos.y);
                        previousRoom.exits[j].connected = true;
                        graphRooms.Add(previousRoom);

                        GetUnconnectedExit();
                        return;
                    }
                }
            }

            roomsToCheck.Remove(i);
        }

        previousRoom.exits[previousExitIndex].connected = true;
        GetUnconnectedExit();
    }

    private void AddBosses() { //WILL be changed
        for (int i = 0; i < bossRooms.Count; i++) {
            if (!bossRooms[i].canPop) {
                continue;
            }

            Room[] rooms = roomsParent[bossRooms[i].zoneIndex].GetComponentsInChildren<Room>();
            int index = FindBossRoom(i, rooms);
            if(index != -1) {
                Destroy(rooms[index].enemiesParent.gameObject);
                GameObject boss = Instantiate(bossRooms[i].boss, rooms[index].transform, false) as GameObject;
                boss.transform.localPosition = new Vector3(boss.transform.localPosition.x * (bossRooms[i].roomSize.x / 2), boss.transform.localPosition.y, boss.transform.localPosition.z); //really ugly WILL be changed
                bossRooms[i].canPop = false;
            }
            else if(bossRooms[i].roomSize.x > 1) {
                bossRooms[i].roomSize.x = 1;
                i--;
            }
            else {
                Application.LoadLevel(0);
                return;
            }
        }
    }

    private int FindBossRoom(int i, Room[] rooms) {
        for (int j = 0; j < rooms.Length; j++) {
            if (rooms[j].size == bossRooms[i].roomSize) {
                return j;
            }
        }

        return -1;
    }

    private void GetNextZone(int x, int y) {   
        if (x >= 0 && x < world.worldSize.x && y >= 0 && y < world.worldSize.y) {
            previousZoneIndex = world.map[x, y].zoneType;
            roomList = zoneRooms[previousZoneIndex].roomList;
        }
    }

    private void GetUnconnectedExit() {

        List<int> exitsTocheck = new List<int>();
        exitsTocheck.AddRange(System.Linq.Enumerable.Range(0, previousRoom.exits.Count));
        while (exitsTocheck.Count > 0) {
            previousExitIndex = exitsTocheck[Random.Range(0, exitsTocheck.Count)];
            Exit exit = previousRoom.exits[previousExitIndex];
            int x = (int)(roomPos.x + exit.pos.x);
            int y = (int)(roomPos.y + exit.pos.y);
            exit.connected = exit.connected || x < 0 || x >= world.worldSize.x || y < 0 || y >= world.worldSize.y || world.map[x, y].hasRoom;
            if (!exit.connected) {
                return;
            }

            exitsTocheck.Remove(previousExitIndex);
        }

        ReturnToPreviousRoom();
        if(graphRooms.Count > 1)
            GetUnconnectedExit();
    }

    private void ReturnToPreviousRoom() {
        graphRooms.Remove(previousRoom);
        if(graphRooms.Count > 1) {
            previousRoom = graphRooms[graphRooms.Count - 1];
            roomPos.Set(previousRoom.pos.x, previousRoom.pos.y);
        }
    }

    private bool RoomHasPlace(Vector2 posStart, Vector2 posEnd) {
        for (int x = (int)posStart.x; x < posEnd.x; x++) {
            for (int y = (int)posStart.y; y < posEnd.y; y++) {
                if(x < 0 || x >= world.worldSize.x
                || y < 0 || y >= world.worldSize.y
                || world.map[x, y].hasRoom) {
                    return false;
                }
            }
        }

        return true;
    }

    private void MarkMapWithRoom(Vector2 posStart, Vector2 posEnd) {
        for (int x = (int)posStart.x; x < posEnd.x; x++) {
            for (int y = (int)posStart.y; y < posEnd.y; y++) {
                world.map[x, y].hasRoom = true;
            }
        }
    }

}
