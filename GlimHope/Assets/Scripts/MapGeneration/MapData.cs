using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MapData
{
    [Serializable]
    public class Tile
    { 
        public int width;
        public int height;
        public Vector2 position;

        public bool leftPassage = false;
        public bool upPassage = false;
        public bool rightPassage = false;
        public bool downPassage = false;

        public int origin = 16;
        public int destination = 16;
        public int roomType = 0;
        public int roomVariation = 99;
        //public bool isStart;
        //public bool isEnd;
        public bool isUsed = false;
        public bool isSolution = false;

        public GameObject room;
    }

    [Serializable]
    public class Index2TileData
    {
        public int index;
        public Tile tileData;
    }

    public int mapWidth;
    public int mapHeight;

    public Index2TileData InitialiseData(int index, int width, int height, Vector2 position,int roomType ,int roomVariation)
    {
        Index2TileData data = new Index2TileData();
        data.index = index;

        Tile tile = new Tile();
        tile.width = width;
        tile.height = height;
        tile.roomType = roomType;
        tile.roomVariation = roomVariation;
        //tile.room = testObject;



        tile.position = position;

        data.tileData = tile;
        
        return data;
    }
}
