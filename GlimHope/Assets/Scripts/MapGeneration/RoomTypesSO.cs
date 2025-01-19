using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomTypesSO", menuName = "Scriptable Objects/RoomTypesSO")]
public class RoomTypesSO : ScriptableObject
{
    [SerializeField] public RoomTypes roomTypes;

    [Serializable]public struct RoomTypes
    {
        [SerializeField] public List<RoomData> pathRooms;
        [SerializeField] public List<RoomData> extraRooms;
        [SerializeField] public List<RoomData> startRooms;
        [SerializeField] public List<RoomData> endRooms;
        [SerializeField] public List<RoomData> blankRooms;
        [SerializeField] public List<RoomData> doorTypes;
        [SerializeField] public GameObject wallType;
    }

    [Serializable] public struct RoomData
    {
        [SerializeField][Tooltip("This should be a percentage out of 100")] public int spawnWeight;
        [SerializeField] public GameObject room;
    }
    
}
