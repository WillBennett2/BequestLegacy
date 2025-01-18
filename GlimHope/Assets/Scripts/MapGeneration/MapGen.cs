using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using static MapData;
using UnityEditor.Experimental.GraphView;

public class MapGen : MonoBehaviour
{
    [Header("Sizes")]
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] int numOfExtraRooms = 2;

    [Header("Gameobject")]
    [SerializeField] private GameObject roomTest;
    [SerializeField] private GameObject pathRoom;
    [SerializeField] private GameObject extraRoom;
    [SerializeField] private GameObject DoorTest;

    private GameObject dungeonContainer;
    private GameObject blockerContainer;
    private GameObject roomContainer;


    [Header("Map Data")]
    public MapData mapData;
    public List<MapData.Index2TileData> map;
    [SerializeField] private List<int> path;
    [SerializeField] private bool showPath;

    private void Start()
    {
        dungeonContainer = new GameObject("DungeonContainer");
        blockerContainer = new GameObject("BlockerContainer");
        blockerContainer.transform.SetParent(dungeonContainer.transform);
        roomContainer = new GameObject("RoomContainer");
        roomContainer.transform.SetParent(dungeonContainer.transform);

        GenerateMap();
    }
    public void GenerateMap()
    {
        InitialiseMap();
        CreatePath();
        PlaceExtraRooms();
        BlockNonPassages();
        CreateVisual();
    }

    private bool CheckPastPath(int newPathIndex)
    {
        foreach (int pathIndex in path)
        {
            if(pathIndex == newPathIndex)
            {
                return false;
            }
        }

        return true;
    }


    private void CreatePath()
    {
        //start at top
        int currentIndex = Random.Range(0, mapWidth);
        path.Add(currentIndex);
        map[currentIndex].tileData.isStart = true;
        map[currentIndex].tileData.isUsed = true;

        if (showPath )
            map[currentIndex].tileData.room = pathRoom;

        bool pathFound=false;
        List<int> options = new List<int> { 0, 1, 2 };

        while (!pathFound)
        {
            int tempCurrentIndex = currentIndex;
            List<int> shuffled = new List<int>(ShuffleList(options));

            foreach (int move in shuffled)
            {
                bool validMove = false;
                switch (move)
                {
                    case 0:// left
                        if(0 <= map[tempCurrentIndex].tileData.position.x - tileWidth && CheckPastPath(currentIndex-1))
                        {
                            //set previous movement
                            map[currentIndex].tileData.leftPassage = true;
                            currentIndex = map[currentIndex].index - 1;
                            map[currentIndex].tileData.rightPassage = true;
                            validMove = true;
                        }

                        break;
                    case 1: //right
                        if (map[tempCurrentIndex].tileData.position.x + tileWidth <= tileWidth*(mapWidth-1) && CheckPastPath(currentIndex+1))
                        {
                            map[currentIndex].tileData.rightPassage = true;
                            currentIndex = map[currentIndex].index + 1;
                            map[currentIndex].tileData.leftPassage = true;
                            validMove = true;
                        }
                        break;

                    case 2: //up
                        if (map[tempCurrentIndex].tileData.position.y + tileHeight <= tileHeight * (mapHeight - 1) && CheckPastPath(currentIndex+ mapHeight))
                        {
                            map[currentIndex].tileData.upPassage = true;
                            currentIndex = map[currentIndex].index + mapHeight;
                            map[currentIndex].tileData.downPassage = true;
                            validMove = true;
                        }
                        break;
                }
                if (validMove)
                {

                    path.Add(currentIndex);
                    map[currentIndex].tileData.isUsed = true;
                    //map[tempCurrentIndex].tile.nextIndex = currentIndex;
                    if (showPath)
                        map[currentIndex].tileData.room = pathRoom;
                    break;
                }
            }
            if(tempCurrentIndex==currentIndex)
            {
                map[tempCurrentIndex].tileData.isEnd = true;

                pathFound = true;
            }
        }
    }
    private List<int> ShuffleList(List<int> options)
    {
        List<int> temp = new List<int>(options);
        List<int> shuffled = new List<int>();


        for (int i = 0; i < options.Count; i++)
        {
            int index = Random.Range(0, temp.Count);
            shuffled.Add(temp[index]);
            temp.Remove(temp[index]);
        }

        return shuffled;
    }
    private List<Index2TileData> ShuffleList(List<Index2TileData> options)
    {
        List<Index2TileData> temp = new List<Index2TileData>(options);
        List<Index2TileData> shuffled = new List<Index2TileData>();


        for (int i = 0; i < options.Count; i++)
        {
            int index = Random.Range(0, temp.Count);
            shuffled.Add(temp[index]);
            temp.Remove(temp[index]);
        }

        return shuffled;
    }

    private void InitialiseTile(int index, Vector2 position)
    {
        map.Add(mapData.InitialiseData(index,tileWidth, tileHeight,position,roomTest));
    }
    private void InitialiseMap() // creates the map from how many tiles and their data
    {
        mapData = new MapData();
        mapData.mapWidth = mapWidth;
        mapData.mapHeight = mapHeight;

        int index = 0;
        Vector2 position = new Vector2();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                InitialiseTile(index,position);
                index++;
                position.x = position.x + tileWidth;
            }
            position.x = 0;
            position.y = position.y + tileHeight;
        }
        
    }

    private void CreateVisual()
    {
        foreach (Index2TileData tile in map)
        {
            Instantiate(tile.tileData.room, tile.tileData.position, Quaternion.identity, roomContainer.transform);
        }
    }

    private void PlaceExtraRooms()
    {

        List<Index2TileData> freeConnectedSpace = new List<Index2TileData>();
        foreach (Index2TileData tile in map)
        {
            bool canAdd = false;
            if (tile.tileData.isUsed)
            {
                continue;
            }

            if (0 <= map[tile.index].tileData.position.x - tileWidth) // check left
            {
                if (map[tile.index - 1].tileData.isUsed)
                    canAdd = true;
            }

            if (map[tile.index].tileData.position.y + tileHeight <= tileHeight * (mapHeight - 1)) //check up 
            {
                if (map[tile.index + mapHeight].tileData.isUsed)
                    canAdd = true;
            }

            if (map[tile.index].tileData.position.x + tileWidth <= tileWidth * (mapWidth - 1)) //check right
            {
                if (map[tile.index + 1].tileData.isUsed)
                    canAdd = true;
            }


            if (0 <= map[tile.index].tileData.position.y - tileHeight) //check down
            {
                if (map[tile.index - mapHeight].tileData.isUsed)
                    canAdd = true;
            }

            if (canAdd)
            {
                freeConnectedSpace.Add(tile);
            }
        }

        List<Index2TileData> pickedSpace = ShuffleList(freeConnectedSpace);

        //b = a != null ? a : b;
        numOfExtraRooms = pickedSpace.Count < numOfExtraRooms ? pickedSpace.Count : numOfExtraRooms;
        for (int i = 0; i < numOfExtraRooms; i++)
        {
            //check left
            //set connection if avaliable 
            if(0 <= map[pickedSpace[i].index].tileData.position.x - tileWidth)
            {
                if (!map[pickedSpace[i].index - 1].tileData.isStart && !map[pickedSpace[i].index - 1].tileData.isEnd && map[pickedSpace[i].index - 1].tileData.isUsed)
                {
                    map[pickedSpace[i].index].tileData.room = extraRoom;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.leftPassage = true;
                    map[pickedSpace[i].index - 1].tileData.rightPassage = true;
                }
            }

            //check up 
            //set connection if avaliable 
            if (map[pickedSpace[i].index].tileData.position.y + tileHeight <= tileHeight * (mapHeight - 1))
            {
                if (!map[pickedSpace[i].index + mapHeight].tileData.isStart && !map[pickedSpace[i].index + mapHeight].tileData.isEnd && map[pickedSpace[i].index + mapHeight].tileData.isUsed)
                {

                    map[pickedSpace[i].index].tileData.room = extraRoom;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.upPassage = true;
                    map[pickedSpace[i].index + mapHeight].tileData.downPassage = true;
                }
            }

            //check right
            //set connection if avaliable 
            if (map[pickedSpace[i].index].tileData.position.x + tileWidth <= tileWidth * (mapWidth - 1))
            {
                if (!map[pickedSpace[i].index + 1].tileData.isStart && !map[pickedSpace[i].index + 1].tileData.isEnd && map[pickedSpace[i].index + 1].tileData.isUsed)
                 {

                    map[pickedSpace[i].index].tileData.room = extraRoom;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.rightPassage = true;
                    map[pickedSpace[i].index + 1].tileData.leftPassage = true;
                }
                
            }

            //check down
            //set connection if avaliable 
            if (0 <= map[pickedSpace[i].index].tileData.position.y - tileHeight)
            {
                if (!map[pickedSpace[i].index - mapHeight].tileData.isStart && !map[pickedSpace[i].index - mapHeight].tileData.isEnd && map[pickedSpace[i].index - mapHeight].tileData.isUsed)
                 {
                    map[pickedSpace[i].index].tileData.room = extraRoom;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.downPassage = true;
                    map[pickedSpace[i].index - mapHeight].tileData.upPassage = true;
                }
                


            }
        }
    }

    private void BlockNonPassages()
    {
        foreach (Index2TileData Tile in map)
        {
            if(!Tile.tileData.upPassage)
            {
                Instantiate(DoorTest, new Vector2( Tile.tileData.position.x, Tile.tileData.position.y+ 7.20f), DoorTest.transform.rotation, blockerContainer.transform);
               
            }
            if (!Tile.tileData.rightPassage)
            {
                Instantiate(DoorTest, new Vector2(Tile.tileData.position.x + 7.20f, Tile.tileData.position.y), Quaternion.Euler(DoorTest.transform.rotation.x, DoorTest.transform.rotation.y, DoorTest.transform.rotation.z), blockerContainer.transform);
            }
            if (!Tile.tileData.downPassage)
            {
                Instantiate(DoorTest, new Vector2(Tile.tileData.position.x, Tile.tileData.position.y - 7.20f), DoorTest.transform.rotation, blockerContainer.transform);
            }
            if (!Tile.tileData.leftPassage)
            {
                Instantiate(DoorTest, new Vector2(Tile.tileData.position.x - 7.20f, Tile.tileData.position.y), Quaternion.Euler( DoorTest.transform.rotation.x, DoorTest.transform.rotation.y, DoorTest.transform.rotation.z), blockerContainer.transform);
            }
        }
    }

}
