using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomObject : MonoBehaviour
{
    [SerializeField] Room roomData = new Room();
    
    
    Vector2 roomSize;
    Camera currentCam = null;
    public void InitRoom(Room _room)
    {
        roomData = _room;
        currentCam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Vector3 _pos = transform.position;
        currentCam.transform.position = new Vector3(_pos.x, _pos.y, -10.0f);
    }
}