using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomObject : MonoBehaviour
{
    [SerializeField] Room roomData = new Room();
    [SerializeField] GameObject doorTop = null;
    [SerializeField] GameObject doorRight = null;
    [SerializeField] GameObject doorLeft = null;
    [SerializeField] GameObject doorDown = null;
    [SerializeField] List<Enemy> enemiesInRoom = null;
    public GameObject DoorTop =>doorTop ;
    public GameObject DoorRight =>doorRight ;
    public GameObject DoorLeft => doorLeft;
    public GameObject DoorDown =>doorDown;
    
    Vector2 roomSize;
    Camera currentCam = null;
    public void InitRoom(Room _room)
    {
        roomData = _room;
        currentCam = Camera.main;
        if (doorTop)
            doorTop.SetActive(!roomData.doorTop);
        if (doorRight)
            doorRight.SetActive(!roomData.doorRight);
        if (doorLeft)
            doorLeft.SetActive(!roomData.doorLeft);
        if (doorDown)
            doorDown.SetActive(!roomData.doorBot);
        GameLogic.Instance.OnResetLevel += LevelReseted;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject != GameLogic.Instance.GetPlayer.gameObject)return;
        Vector3 _pos = transform.position;
        currentCam.transform.position = new Vector3(_pos.x, _pos.y, -10.0f);
        Minimap.Instance.RoomDiscovered(this);
        foreach (Enemy _enemy in enemiesInRoom)
            _enemy.Activate();
        GameLogic.Instance.NewCurrentRoom(this);
    }

    public void DeactivateRoom()
    {
        foreach (Enemy _enemy in enemiesInRoom)
            _enemy.Deactivate();
    }
    
    void LevelReseted()
    {
        Destroy(gameObject);
    }
}