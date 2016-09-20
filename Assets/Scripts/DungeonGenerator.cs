using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour {

    public Transform roomsParent;
    public Vector2 startingPos;
    public Room startingRoom;
    public List<Room> rooms;
    public List<Room> deadEndRooms;
    public int minRoomsBeforeDeadEnd = 10;
    public float secondsBetweenInstanciation = 2;

    private World world;
    private Room previousRoom;
    private List<Room> graphRooms = new List<Room>();
    private int previousExitIndex;
    private Vector2 roomPos = Vector3.zero;
    private Vector2 nextRoomPos = Vector3.zero;
    private Vector3 roomWorldPos;
    private int currentRoomsNb;
    private bool deadEndAdded = false;

    // Use this for initialization
    void Start () {
        world = World.instance;
        roomPos = startingPos;
        roomWorldPos.Set(roomPos.x * world.roomBaseSize.x, roomPos.y * world.roomBaseSize.y, 0);

        previousRoom = Instantiate(startingRoom, roomWorldPos, Quaternion.identity, roomsParent) as Room;
        graphRooms.Add(previousRoom);
        previousExitIndex = 0; //assumes that the starting room only has one exit
        MarkMapWithRoom(roomPos, roomPos + previousRoom.size);

        StartCoroutine(Generation());
    }

    IEnumerator Generation() {
        do {
           if (secondsBetweenInstanciation > 0)
                yield return new WaitForSeconds(secondsBetweenInstanciation);

           if(!deadEndAdded && currentRoomsNb >= minRoomsBeforeDeadEnd) {
                deadEndAdded = true;
                rooms.AddRange(deadEndRooms);
            }

            GenerateRoom();

        } while (graphRooms.Count > 1);

        Debug.Log(Time.realtimeSinceStartup);
        yield return null;
    }
       

    private void GenerateRoom() {
        List<int> roomsToCheck = new List<int>();
        roomsToCheck.AddRange(System.Linq.Enumerable.Range(0, rooms.Count)); // so that we have a list like [0,1,2,3 ... count-1]
        while(roomsToCheck.Count > 0) {
            int i = roomsToCheck[Random.Range(0, roomsToCheck.Count)];
            for (int j = 0; j < rooms[i].exits.Count; j++) {
                if (rooms[i].exits[j].dir * -1 == previousRoom.exits[previousExitIndex].dir) {
                    // ok it's complicated. I'm pretty sure there's an easier way to do that.
                    // basicly it set the new pos of the room. Since the previous room exit gives the possition of the next room it should be easy right? WRONG
                    // because I have to take into acount the offset of the chosen exit of the new room
                    // and tey don't have the same refenrenctiel at all!
                    // so I calculated the place the current room exit should be (by reversing the dir of the previous room)
                    // then I calculated where the exit will actually be (exit pos of the previous room + exit pos of the current room)
                    // now I can do a simple substraction and get the offset!
                    // man I really suck at those things...

                   // Debug.Log("previousExitIndex: " + previousExitIndex);
                    //Debug.Log("currentExit: " + j);

                    nextRoomPos.x = roomPos.x + previousRoom.exits[previousExitIndex].pos.x - ((previousRoom.exits[previousExitIndex].pos.x + rooms[i].exits[j].pos.x) - (previousRoom.exits[previousExitIndex].pos.x - 1 * previousRoom.exits[previousExitIndex].dir.x)); 
                    nextRoomPos.y = roomPos.y + previousRoom.exits[previousExitIndex].pos.y - ((previousRoom.exits[previousExitIndex].pos.y + rooms[i].exits[j].pos.y) - (previousRoom.exits[previousExitIndex].pos.y - 1 * previousRoom.exits[previousExitIndex].dir.y));

                    roomWorldPos.Set(roomPos.x * world.roomBaseSize.x, roomPos.y * world.roomBaseSize.y, 0);
                   /* Room tmpRoom = Instantiate(rooms[i], roomWorldPos, Quaternion.identity, roomsParent) as Room;
                    tmpRoom.transform.GetChild(0).GetComponent<UnityEngine.TileMap.TileMap>().color = Color.red;*/

                    if (RoomHasPlace(nextRoomPos, nextRoomPos + rooms[i].size)) {
                       // Destroy(tmpRoom.gameObject);

                        previousRoom.exits[previousExitIndex].connected = true;
                        currentRoomsNb++;
                        roomPos.Set(nextRoomPos.x, nextRoomPos.y);
                        MarkMapWithRoom(roomPos, roomPos + rooms[i].size);

                        roomWorldPos.Set(roomPos.x * world.roomBaseSize.x, roomPos.y * world.roomBaseSize.y, 0);
                        previousRoom = Instantiate(rooms[i], roomWorldPos, Quaternion.identity, roomsParent) as Room;
                        previousRoom.pos.Set(roomPos.x, roomPos.y);
                        previousRoom.exits[j].connected = true;
                        graphRooms.Add(previousRoom);

                        GetUnconnectedExit();
                        return;
                    }

                    /*else {
                        yield return new WaitForSeconds(secondsBetweenInstanciation);
                        Destroy(tmpRoom.gameObject);
                    }*/
                }
            }

            roomsToCheck.Remove(i);
        }

        GetUnconnectedExit();
    }

    private void GetUnconnectedExit() {
        List<int> exitsTocheck = new List<int>();
        exitsTocheck.AddRange(System.Linq.Enumerable.Range(0, previousRoom.exits.Count));
        while (exitsTocheck.Count > 0) {
            previousExitIndex = exitsTocheck[Random.Range(0, exitsTocheck.Count)];
            Exit exit = previousRoom.exits[previousExitIndex];
            int x = (int)(roomPos.x + exit.pos.x);
            int y = (int)(roomPos.y + exit.pos.y);
            exit.connected = x < 0 || x >= world.worldSize.x || y < 0 || y >= world.worldSize.y || world.map[x, y].hasRoom;
            if (!previousRoom.exits[previousExitIndex].connected) {
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
