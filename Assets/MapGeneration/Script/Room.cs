using UnityEngine;

[System.Serializable]
public class Room
{
    [SerializeField] public Vector2 gridPos;
    public int type;
    public bool doorTop, doorBot, doorLeft, doorRight;

    public Room(Vector2 _gridPos, int _type)
    {
        gridPos = _gridPos;
        type = _type;
        doorTop = doorBot = doorLeft = doorRight = false;
    }

    public Room()
    {
        
    }
}