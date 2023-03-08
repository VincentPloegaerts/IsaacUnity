using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Vector2 worldSize = new Vector2(5, 5);
    [SerializeField] int numberOfRooms = 25;
    [SerializeField] float randomCompareMax = 0.2f;
    [SerializeField] float randomCompareMin = 0.01f;
    Room[,] rooms;
    [SerializeField] List<Vector2> takenPositions = new List<Vector2>(); 
    int gridSizeX = 0;      //half extents
    int gridSizeY = 0;

    float randomCompare = 0.2f;
    void Start()
    {
        if (numberOfRooms >= worldSize.x * worldSize.y * 4) // make sure we dont try to make more rooms than can fit in our grid
            numberOfRooms = Mathf.RoundToInt(worldSize.x  * worldSize.y * 4);
        gridSizeX = Mathf.RoundToInt(worldSize.x); 
        gridSizeY = Mathf.RoundToInt(worldSize.y);
        CreateRooms(); //lays out the actual map
        //SetRoomDoors(); //assigns the doors where rooms would connect
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
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare){
                int iterations = 0;
                do{
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
        do{
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
    
    Vector2 SelectiveNewPosition(){ // method differs from the above in the two commented ways
        int index = 0, inc = 0;
        int x =0, y =0;
        Vector2 checkingPos = Vector2.zero;
        do{
            inc = 0;
            do{ 
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

    void OnDrawGizmos()
    {
        for (int i = 0; i < takenPositions.Count; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(takenPositions[i],0.2f);
            
        }
    }
}
