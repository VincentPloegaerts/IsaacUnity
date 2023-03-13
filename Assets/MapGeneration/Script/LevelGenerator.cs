using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MySingleton<LevelGenerator>
{
    [SerializeField] Vector2 worldSize = new Vector2(5, 5);
    [SerializeField] int numberOfRooms = 25;
    [SerializeField] float randomCompareMax = 0.2f;
    [SerializeField] float randomCompareMin = 0.01f;
    [SerializeField] List<Vector2> takenPositions = new List<Vector2>();    
    [SerializeField] Vector2 roomDimensions = new Vector2(16*17,16*9);
    [SerializeField] Vector2 gutterSize = new Vector2(16*9,16*4);
    [SerializeField] List<RoomObject> roomPrefabList = new List<RoomObject>();
    Room[,] rooms;
    int gridSizeX = 0;      //half extents
    int gridSizeY = 0;

    float randomCompare = 0.2f;
    
    public Vector2 RoomDimensions => roomDimensions;
    void Start()
    {
        if (numberOfRooms >= worldSize.x * worldSize.y * 4) // make sure we dont try to make more rooms than can fit in our grid
            numberOfRooms = Mathf.RoundToInt(worldSize.x  * worldSize.y * 4);
        gridSizeX = Mathf.RoundToInt(worldSize.x); 
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms(); //lays out the actual map
        SetRoomDoors(); //assigns the doors where rooms would connect
        SpawnRooms(rooms);
        //DrawMap(); //instantiates objects to make up a map


    }

    void CreateRooms()
    {
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, 1); //Init a first room to go from
        takenPositions.Clear();
        takenPositions.Insert(0,Vector2.zero);
        Vector2 checkPos = Vector2.zero;
        //add rooms
        for (int i =0; i < numberOfRooms -1; i++)
        {
            float randomPerc =  i / (numberOfRooms - 1.0f);
            randomCompare = Mathf.Lerp(randomCompareMax, randomCompareMin, randomPerc);
            //grab new position
            checkPos = NewPosition();
            //test new position
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                }while(NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //finalize position
            rooms[(int) checkPos.x + gridSizeX, (int) checkPos.y + gridSizeY] = new Room(checkPos, 0);
            takenPositions.Insert(0,checkPos);
        }	
    }
    
    Vector2 NewPosition() 
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        int _max = takenPositions.Count - 1;
        do
        {
            int index = Mathf.RoundToInt(Random.value * _max); // pick a random room
            x = (int) takenPositions[index].x;//and its positions
            y = (int) takenPositions[index].y;
            if (Random.value < 0.5f) //add a random direction to go to
                y += Random.value < 0.5f ? 1 : -1;
            else
                x += Random.value < 0.5f ? 1 : -1;
            checkingPos = new Vector2(x,y);
        }while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
        return checkingPos;
    }
    
    Vector2 SelectiveNewPosition()  //Same logic as NewPosition but takes more time to ensure some branching
    { 
        int index = 0, inc = 0;
        int x =0, y =0;
        Vector2 checkingPos;
        do
        {
            inc = 0;
            do
            { 
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));            // Take a a room with no neighbours
                inc ++;                                                                         //so its more likely to do paths instead of blocks
            }while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100); 
            x = (int) takenPositions[index].x;
            y = (int) takenPositions[index].y;
            if (Random.value < 0.5f)
                y += Random.value < 0.5f ? 1 : -1;
            else
                x += Random.value < 0.5f ? 1 : -1;
            checkingPos = new Vector2(x,y);
        }while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }
    
    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions){
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2.right))
            ret++;
        if (usedPositions.Contains(checkingPos + Vector2.left))
            ret++;
        if (usedPositions.Contains(checkingPos + Vector2.up))
            ret++;
        if (usedPositions.Contains(checkingPos + Vector2.down))
            ret++;
        return ret;
    }

    void SetRoomDoors()
    {
        for (int x = 0; x < gridSizeX * 2; x++)
        {
            for (int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x,y] == null)
                    continue;
                if (y - 1 < 0)                          //check above
                    rooms[x,y].doorBot = false;
                else
                    rooms[x,y].doorBot = rooms[x,y-1] != null;
                if (y + 1 >= gridSizeY * 2)             //check bellow
                    rooms[x,y].doorTop = false;
                else
                    rooms[x,y].doorTop = rooms[x,y+1] != null;
                if (x - 1 < 0)                          //check left
                    rooms[x,y].doorLeft = false;
                else
                    rooms[x,y].doorLeft = rooms[x - 1,y] != null;
                if (x + 1 >= gridSizeX * 2)             //check right
                    rooms[x,y].doorRight = false;
                else
                    rooms[x,y].doorRight = rooms[x+1,y] != null;
            }
        }
    }

    void SpawnRooms(Room[,] _rooms)
    {
        foreach (Room room in _rooms){
            if (room == null)
                continue;                   //there's no room on this tile
            
            int index = Mathf.RoundToInt(Random.value * (roomPrefabList.Count -1));
            Vector3 pos = new Vector3(room.gridPos.x * roomDimensions.x, room.gridPos.y * roomDimensions.y, 0); //find position to place room
            RoomObject _currentRoom = Instantiate(roomPrefabList[index], pos, Quaternion.identity);
            _currentRoom.InitRoom(room);
        }
    }
    
    void OnDrawGizmos()
    {
        // return;
        // for (int i = 0; i < takenPositions.Count; i++)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawSphere(takenPositions[i],0.2f);
        //     Texture2D _tex;
        // }
    }
}
