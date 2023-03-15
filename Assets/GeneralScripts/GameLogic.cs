using System;
using UnityEngine;

public class GameLogic : MySingleton<GameLogic>
{
    public event Action OnResetLevel = null;
    [SerializeField] Player playerRef = null;
    [SerializeField] RoomObject currentRoom = null;

    public Player GetPlayer => playerRef;

    public void ResetLevel()
    {
        OnResetLevel?.Invoke();
    }

    public void NewCurrentRoom(RoomObject _room)
    {
        if (currentRoom)
            currentRoom.DeactivateRoom();
        currentRoom = _room;
    }
}