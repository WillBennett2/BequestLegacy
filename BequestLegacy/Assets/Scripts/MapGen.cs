using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using static MapData;
using UnityEditor.Experimental.GraphView;

public class MapGen : MonoBehaviour
{ 
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private GameObject roomTest;
    [SerializeField] private GameObject pathRoom;
    [SerializeField] private GameObject DoorTest;


    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;


    public MapData mapData;
    public List<MapData.Index2TileData> map;
    [SerializeField] private List<int> path;
    [SerializeField] private bool showPath;

    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        /*generation steps
        
        initialise a tile
        create map with tiles

        create a solution path

        Fill in solution path

        Fill rest of dungeon


        */
        InitialiseMap();
        CreatePath();
        CreateVisual();
        PlaceExtraRooms();
        BlockNonPassages();
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
                        if (map[tempCurrentIndex].tileData.position.x + tileWidth <= 48 && CheckPastPath(currentIndex+1))
                        {
                            map[currentIndex].tileData.rightPassage = true;
                            currentIndex = map[currentIndex].index + 1;
                            map[currentIndex].tileData.leftPassage = true;
                            validMove = true;
                        }
                        break;

                    case 2: //up
                        if (map[tempCurrentIndex].tileData.position.y + tileHeight <= 48 && CheckPastPath(currentIndex+4))
                        {
                            map[currentIndex].tileData.upPassage = true;
                            currentIndex = map[currentIndex].index + 4;
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
                map[tempCurrentIndex].tileData.isEnd = false;
                pathFound = true;
            }
        }


        // place end-
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
            Instantiate(tile.tileData.room, tile.tileData.position, Quaternion.identity);
        }
    }

    private void PlaceExtraRooms()
    {

        foreach(Index2TileData tile in map)
        {
            if(!tile.tileData.isUsed)
            {
                //check left
                //set connection if avaliable 
                if(0 <= map[tile.index].tileData.position.x - tileWidth)
                {

                }


                //check up 
                //set connection if avaliable 

                //check right
                //set connection if avaliable 

                //check down
                //set connection if avaliable 

            }

        }

    }

    private void BlockNonPassages()
    {
        foreach (Index2TileData Tile in map)
        {
            if(!Tile.tileData.upPassage)
            {
                Instantiate(DoorTest, new Vector2( Tile.tileData.position.x, Tile.tileData.position.y+ 7.20f), DoorTest.transform.rotation);
               
            }
            if (!Tile.tileData.rightPassage)
            {
                Instantiate(DoorTest, new Vector2(Tile.tileData.position.x + 7.20f, Tile.tileData.position.y), Quaternion.Euler(DoorTest.transform.rotation.x, DoorTest.transform.rotation.y, DoorTest.transform.rotation.z));
            }
            if (!Tile.tileData.downPassage)
            {
                Instantiate(DoorTest, new Vector2(Tile.tileData.position.x, Tile.tileData.position.y - 7.20f), DoorTest.transform.rotation);
            }
            if (!Tile.tileData.leftPassage)
            {
                Instantiate(DoorTest, new Vector2(Tile.tileData.position.x - 7.20f, Tile.tileData.position.y), Quaternion.Euler( DoorTest.transform.rotation.x, DoorTest.transform.rotation.y, DoorTest.transform.rotation.z));
            }
        }
    }

}
