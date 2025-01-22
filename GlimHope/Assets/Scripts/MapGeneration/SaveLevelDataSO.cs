using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLevelDataSO", menuName = "Scriptable Objects/SaveLevelDataSO")]
public class SaveLevelDataSO : ScriptableObject
{
    [Serializable]public class SaveDataInfo
    {
        public List<int> data = new List<int>();
    }

    [SerializeField] public List<SaveDataInfo> levelData = new List<SaveDataInfo>();


    public void SaveData(List<int> data)
    {
        List<int> tileData = new List<int>();

        //int index = 0;
        //if(0 < levelData.Count)
        //    index = levelData.Count-1;

        //Debug.Log(index);
        foreach (int tile in data)
        {
            tileData.Add(tile);
        }
        SaveDataInfo saveDataInfo = new SaveDataInfo();
        saveDataInfo.data = tileData;
        levelData.Add(saveDataInfo);
    }
}
