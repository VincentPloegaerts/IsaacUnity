using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] Room roomData = new Room();

    public void InitRoom(Room _room)
    {
        roomData = _room;
    }
}