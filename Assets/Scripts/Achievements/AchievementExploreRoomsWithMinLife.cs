using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementExploreRoomsWithMinLife : BaseAchievement {

    [SerializeField] private float minLifePercent;
    [SerializeField] private int roomsNeededNb;

    private List<GraphRoom> exploredRooms = new List<GraphRoom>();

    protected override void Start () {
        base.Start();

        EventDispatcher.AddEventListener(Events.PLAYER_HIT, OnPlayerHit);
        EventDispatcher.AddEventListener(Events.PLAYER_ENTER_ROOM, OnPlayerEnterRoom);
    }

    void OnDestroy () {
        EventDispatcher.RemoveEventListener(Events.PLAYER_HIT, OnPlayerHit);
        EventDispatcher.RemoveEventListener(Events.PLAYER_ENTER_ROOM, OnPlayerEnterRoom);
    }

    private void OnPlayerHit(object playerObj) {
        Player player = (Player)playerObj;
        if (player.currentLife < player.maxLife * minLifePercent) {
            Destroy(this);
        }
    }

    private void OnPlayerEnterRoom(object roomObj) {
        GraphRoom room = (GraphRoom)roomObj;
        if(!exploredRooms.Contains(room)) {
            exploredRooms.Add(room);
            if(exploredRooms.Count >= roomsNeededNb) {
                Unlock();
            }
        }
    }
}
