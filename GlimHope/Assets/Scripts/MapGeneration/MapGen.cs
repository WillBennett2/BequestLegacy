using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using static MapData;
using UnityEditor.Experimental.GraphView;
using System.Collections;
using System.Linq;
using static UnityEngine.EventSystems.EventTrigger;

public class MapGen : MonoBehaviour
{
    [Header("Sizes")]
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField][Tooltip("This value will reduce to the maximum number of viable extra rooms on runtime")] int numOfExtraRooms = 2;
    [SerializeField] private int minPathSize = 5;
    [SerializeField] private int maxPathSize = 13;

    [Header("Gameobject")]
    [SerializeField] private RoomTypesSO roomData;

    private GameObject dungeonContainer;
    private GameObject blockerContainer;
    private GameObject roomContainer;


    [Header("Map Data")]
    public MapData mapData;
    public List<MapData.Index2TileData> map;
    [SerializeField] private List<int> path;
    [SerializeField] private bool showPath;

    private SaveMapData saveMapData;
    private LoadMapData loadMapData;

    private void Start()
    {
        loadMapData = new LoadMapData();
        saveMapData = new SaveMapData();
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateMap();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetLastGeneration();
            loadMapData.LoadMapLayout(saveMapData.mapData);
            LoadMap(loadMapData.directionMap, loadMapData.destinationMap ,loadMapData.mapTypes, loadMapData.mapVariation);
            Debug.Log("Reloaded");
        }
    } 

    public void GenerateMap()
    {

        ResetLastGeneration();
        CreateHolders();
        InitialiseMap();
        CreatePath();
        PlaceExtraRooms();
        BlockNonPassages();
        CreateVisual();

        if(path.Count<minPathSize || path.Count > maxPathSize)
        {
            Debug.Log("Dungeon was too extreme");
            GenerateMap();
        }

        saveMapData.SaveMapLayout(map);
    }
    private void ResetLastGeneration()
    {
        if (dungeonContainer != null)
        {
            Destroy(blockerContainer);
            Destroy(roomContainer);
            Destroy(dungeonContainer);
        }

        dungeonContainer = null;
        blockerContainer = null;
        roomContainer = null;

        map.Clear();
        path.Clear();
    }

    private void CreateHolders()
    {
        dungeonContainer = new GameObject("DungeonContainer");
        blockerContainer = new GameObject("BlockerContainer");
        blockerContainer.transform.SetParent(dungeonContainer.transform);
        roomContainer = new GameObject("RoomContainer");
        roomContainer.transform.SetParent(dungeonContainer.transform);
    }
    private void InitialiseTile(int index, Vector2 position)
    {
        map.Add(mapData.InitialiseData(index, tileWidth, tileHeight, position, 0, GetBlankRoomType()));
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
                InitialiseTile(index, position);
                index++;
                position.x = position.x + tileWidth;
            }
            position.x = 0;
            position.y = position.y + tileHeight;
        }

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

    private void LoadMap(List<int> directionMap, List<int> destinationMap,List<int> mapTypes, List<int> mapVariation)
    {
        CreateHolders();
        InitialiseMap();
        CreatePath(directionMap, destinationMap, mapTypes, mapVariation);
        BlockNonPassages();
        CreateVisual();
    }
    
    private void CreatePath(List<int> directionMap, List<int> destinationMap, List<int> mapTypes, List<int> mapVariation)
    {
        for (int i = 0; i <= 4; i++) 
        {
            if(directionMap[i]== 5 )
            {
                path.Add(i);
                map[i].tileData.origin = 99;
                map[i].tileData.roomType = 3;
                map[i].tileData.isUsed = true;
                map[i].tileData.isSolution = true;
                map[i].tileData.roomVariation = GetEndType();
            }
        }

        //save path into shuffled
        LoadAssignPath(path[0], directionMap, destinationMap, mapTypes, mapVariation, false);
    }
    private void LoadAssignPath(int currentIndex, List<int> options, List<int> destinationMap, List<int> mapTypes, List<int> mapVariation, bool shuffle)
    {
        for (int i = 0; i < options.Count; i++)
        {
            //set the 
            switch (options[i])
            {
                case 1: // came from left
                    map[i].tileData.rightPassage = true;
                    map[i].tileData.origin = i+1;
                    break;
                case 2:// came from right
                    map[i].tileData.leftPassage = true;
                    map[i].tileData.origin = i-1;
                    break;
                case 3: // came from below
                    map[i].tileData.downPassage = true;
                    map[i].tileData.origin = i-4;
                    break;
                default:
                    break;
            }
            switch (destinationMap[i])
            {
                case 1: // went left
                    map[i].tileData.rightPassage = true;
                    map[i].tileData.destination = i-1;
                    break;
                case 2:// went right
                    map[i].tileData.leftPassage = true;
                    map[i].tileData.destination = i+1;
                    break;
                case 3: // went up
                    map[i].tileData.upPassage = true;
                    map[i].tileData.destination = i+4;
                    break;
                default:
                    break;
            }

            if (mapTypes[i] != 0)
            {
                path.Add(i);
                map[i].tileData.isUsed = true;
                map[i].tileData.isSolution = true;
                map[i].tileData.roomType = mapTypes[i];
                map[i].tileData.roomVariation = mapVariation[i];
            }
        }
        for (int i = 0; i < options.Count; i++) //placing extras
        {
            if (mapTypes[i] == 2)
            {
                if (0<= i-1 && 0 <= map[i].tileData.position.x - tileWidth) //left
                {
                    if (map[i - 1].tileData.isUsed == true && map[i - 1].tileData.roomType != 3 && map[i - 1].tileData.roomType != 4)
                    {
                        map[i].tileData.leftPassage = true;
                        map[i - 1].tileData.rightPassage = true;
                    }
                }
                if (i+1 <= options.Count && map[i].tileData.position.x + tileWidth <= tileWidth * (mapWidth - 1)) //right
                {
                    if (map[i + 1].tileData.isUsed == true && map[i + 1].tileData.roomType != 3 && map[i + 1].tileData.roomType != 4)
                    {
                        map[i].tileData.rightPassage = true;
                        map[i + 1].tileData.leftPassage = true;
                    }
                }
                if (i+ mapHeight <= options.Count && map[i].tileData.position.y + tileHeight <= tileHeight * (mapHeight - 1)) //up
                {
                    if (map[i + mapHeight].tileData.isUsed == true && map[i + mapHeight].tileData.roomType != 3 && map[i + mapHeight].tileData.roomType != 4)
                    {
                        map[i].tileData.upPassage = true;
                        map[i + mapHeight].tileData.downPassage = true;
                    }
                }
                if (0 <= i - mapHeight && 0 <= map[i].tileData.position.y - tileHeight) //down
                {
                    if (map[i - mapHeight].tileData.isUsed == true && map[i - mapHeight].tileData.roomType != 3 && map[i - mapHeight].tileData.roomType != 4)
                    {
                        map[i].tileData.downPassage = true;
                        map[i - mapHeight].tileData.upPassage = true;
                    }
                }

                map[i].tileData.isUsed = true;
                map[i].tileData.roomType = mapTypes[i];
                map[i].tileData.roomVariation = mapVariation[i];
            }
        }
    }
    private void CreatePath()
    {
        int currentIndex = Random.Range(0, mapWidth);
        path.Add(currentIndex);
        map[currentIndex].tileData.origin = 99;
        map[currentIndex].tileData.roomType = 3;
        map[currentIndex].tileData.roomVariation = GetEndType();
        map[currentIndex].tileData.isUsed = true;
        map[currentIndex].tileData.isSolution = true;

        List<int> options = new List<int> { 1, 2, 3 };
        AssignPath(currentIndex, options, null, true);
    }
    private void AssignPath(int currentIndex, List<int> options, List<int> mapVariation, bool shuffle)
    {
        bool pathFound = false;
        bool skipCheck = false;



        //while (!pathFound)
        for (int i = 0; i < 16; i++)
        {
            skipCheck = false;
            int tempCurrentIndex = currentIndex;
            List<int> shuffled;
            if (shuffle)
            {

                shuffled = new List<int>(ShuffleList(options));
            }
            else
            {
                shuffled = options;
                Debug.Log(shuffled.Count);
            }
            foreach (int move in shuffled)
            {
                if (!shuffle)
                {
                    Debug.Log(move);
                }
                bool validMove = false;
                switch (move)
                {
                    case 1:// left
                        if (0 <= map[tempCurrentIndex].tileData.position.x - tileWidth && CheckPastPath(currentIndex - 1))
                        {
                            //set previous movement
                            map[currentIndex].tileData.leftPassage = true;
                            currentIndex = map[currentIndex].index - 1;
                            map[currentIndex].tileData.rightPassage = true;
                            validMove = true;
                        }

                        break;
                    case 2: //right
                        if (map[tempCurrentIndex].tileData.position.x + tileWidth <= tileWidth * (mapWidth - 1) && CheckPastPath(currentIndex + 1))
                        {
                            map[currentIndex].tileData.rightPassage = true;
                            currentIndex = map[currentIndex].index + 1;
                            map[currentIndex].tileData.leftPassage = true;
                            validMove = true;
                        }
                        break;

                    case 3: //up
                        if (map[tempCurrentIndex].tileData.position.y + tileHeight <= tileHeight * (mapHeight - 1) && CheckPastPath(currentIndex + mapHeight))
                        {
                            map[currentIndex].tileData.upPassage = true;
                            currentIndex = map[currentIndex].index + mapHeight;
                            map[currentIndex].tileData.downPassage = true;
                            validMove = true;
                        }
                        break;

                    case 5:
                        skipCheck = true;
                        break;
                    case 6:
                        skipCheck = true;
                        break;
                }
                if (validMove)
                {
                    path.Add(currentIndex);
                    map[currentIndex].tileData.isUsed = true;
                    map[currentIndex].tileData.isSolution = true;
                    map[currentIndex].tileData.origin = tempCurrentIndex;
                    map[tempCurrentIndex].tileData.destination = currentIndex;
                    map[currentIndex].tileData.roomType = 1;

                    if (shuffle)
                    {
                        map[currentIndex].tileData.roomVariation = GetPathRoomType();
                    }
                    else
                    {
                        map[currentIndex].tileData.roomVariation = mapVariation[currentIndex];
                    }

                    break;
                }
            }
            if (tempCurrentIndex == currentIndex)
            {
                map[tempCurrentIndex].tileData.roomType = 4;
                map[tempCurrentIndex].tileData.destination = 99;
                map[tempCurrentIndex].tileData.isSolution = true;
                map[currentIndex].tileData.roomVariation = GetEndType();

                pathFound = true;
                break;
            }

            if(pathFound)
            {
                i = 16;
            }
        }
    }

    private void CreateVisual()
    {
        foreach (Index2TileData tile in map)
        {
            switch (tile.tileData.roomType)
            {
                case 0:
                    tile.tileData.room = roomData.roomTypes.blankRooms[tile.tileData.roomVariation].room;
                    break;
                case 1:
                    tile.tileData.room = roomData.roomTypes.pathRooms[tile.tileData.roomVariation].room;
                    break;
                case 2:
                    tile.tileData.room = roomData.roomTypes.extraRooms[tile.tileData.roomVariation].room;
                    break;
                case 3:
                    tile.tileData.room = roomData.roomTypes.startRooms[tile.tileData.roomVariation].room;
                    break;
                case 4:
                    tile.tileData.room = roomData.roomTypes.endRooms[tile.tileData.roomVariation].room;
                    break;
                default:
                    break;
            }

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
        int maxExtraRooms = pickedSpace.Count < numOfExtraRooms ? pickedSpace.Count : numOfExtraRooms;
        for (int i = 0; i < maxExtraRooms; i++)
        {
            bool assignRoom = false;
            if (0 <= map[pickedSpace[i].index].tileData.position.x - tileWidth)            //check left
            {
                if (map[pickedSpace[i].index - 1].tileData.roomType != 3 && map[pickedSpace[i].index - 1].tileData.roomType != 4 && map[pickedSpace[i].index - 1].tileData.isUsed)
                {
                    assignRoom = true;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.leftPassage = true;
                    map[pickedSpace[i].index - 1].tileData.rightPassage = true;
                    //map[pickedSpace[i].index].tileData.origin = pickedSpace[i].index - 1;
                }
            }

            if (map[pickedSpace[i].index].tileData.position.y + tileHeight <= tileHeight * (mapHeight - 1))            //check up 
            {
                if (map[pickedSpace[i].index + mapHeight].tileData.roomType != 3 && map[pickedSpace[i].index + mapHeight].tileData.roomType != 4 && map[pickedSpace[i].index + mapHeight].tileData.isUsed)
                {

                    assignRoom = true;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.upPassage = true;
                    map[pickedSpace[i].index + mapHeight].tileData.downPassage = true;
                    //map[pickedSpace[i].index].tileData.origin = pickedSpace[i].index + mapHeight;
                }
            }

            if (map[pickedSpace[i].index].tileData.position.x + tileWidth <= tileWidth * (mapWidth - 1))            //check right
            {
                if (map[pickedSpace[i].index + 1].tileData.roomType != 3 && map[pickedSpace[i].index + 1].tileData.roomType != 4 && map[pickedSpace[i].index + 1].tileData.isUsed)
                 {

                    assignRoom = true;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.rightPassage = true;
                    map[pickedSpace[i].index + 1].tileData.leftPassage = true;
                    //map[pickedSpace[i].index].tileData.origin = pickedSpace[i].index + 1;
                }
                
            }

            if (0 <= map[pickedSpace[i].index].tileData.position.y - tileHeight)            //check down
            {
                if (map[pickedSpace[i].index - mapHeight].tileData.roomType != 3 && map[pickedSpace[i].index - mapHeight].tileData.roomType != 4 && map[pickedSpace[i].index - mapHeight].tileData.isUsed)
                 {
                    assignRoom = true;
                    map[pickedSpace[i].index].tileData.isUsed = true;
                    map[pickedSpace[i].index].tileData.downPassage = true;
                    map[pickedSpace[i].index - mapHeight].tileData.upPassage = true;
                    //map[pickedSpace[i].index].tileData.origin = pickedSpace[i].index - mapHeight;
                }
            }

            if(assignRoom)
            {
                map[pickedSpace[i].index].tileData.roomType = 2;
                map[pickedSpace[i].index].tileData.roomVariation = GetExtraRoomType();
            }
        }
    }

    private void BlockNonPassages()
    {
        GameObject wall = GetWall();
        foreach (Index2TileData Tile in map)
        {
            if(!Tile.tileData.upPassage)
            {
                Instantiate(wall, new Vector2( Tile.tileData.position.x, Tile.tileData.position.y+ 7.20f), wall.transform.rotation, blockerContainer.transform);
               
            }
            if (!Tile.tileData.rightPassage)
            {
                Instantiate(wall, new Vector2(Tile.tileData.position.x + 7.20f, Tile.tileData.position.y), Quaternion.Euler(wall.transform.rotation.x, wall.transform.rotation.y, wall.transform.rotation.z), blockerContainer.transform);
            }
            if (!Tile.tileData.downPassage)
            {
                Instantiate(wall, new Vector2(Tile.tileData.position.x, Tile.tileData.position.y - 7.20f), wall.transform.rotation, blockerContainer.transform);
            }
            if (!Tile.tileData.leftPassage)
            {
                Instantiate(wall, new Vector2(Tile.tileData.position.x - 7.20f, Tile.tileData.position.y), Quaternion.Euler(wall.transform.rotation.x, wall.transform.rotation.y, wall.transform.rotation.z), blockerContainer.transform);
            }
        }
    }

    #region Extra Utility
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


    private int GetBlankRoomType()
    {
        return WeightedChoice(roomData.roomTypes.blankRooms);
    }

    private int GetPathRoomType()
    {
        return WeightedChoice(roomData.roomTypes.pathRooms);
    }

    private int GetExtraRoomType()
    {
        return WeightedChoice(roomData.roomTypes.extraRooms);
    }

    private GameObject GetWall()
    {
       return roomData.roomTypes.wallType;
    }

    private int GetStartType()
    {
        return WeightedChoice(roomData.roomTypes.startRooms);
    }
    private int GetEndType()
    {
        return WeightedChoice(roomData.roomTypes.endRooms);
    }

    private int WeightedChoice(List<RoomTypesSO.RoomData> entries)
    {
        int totalWeight = entries.Sum(x => x.spawnWeight);
        int wantedWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;

        for (int i = 0; i < entries.Count; i++)
        {
            currentWeight += entries[i].spawnWeight;
            if (currentWeight >= wantedWeight)
            {
                return i;
            }
        }

        return -1;
    }
    #endregion
}
