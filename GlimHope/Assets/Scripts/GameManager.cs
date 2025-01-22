using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level")]
    public int level;

    [Header("Saved Data")]
    [SerializeField] public SaveLevelDataSO savedData;
    private SaveMapData saveMapData;
    private LoadMapData loadMapData;

    private void Awake()
    {
        savedData.levelData.Clear();
        loadMapData = new LoadMapData();
        saveMapData = new SaveMapData();
        saveMapData.SetSaveDataSO(savedData);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData(List<MapData.Index2TileData> map)
    {
        saveMapData.SaveMapLayout(map);
    }
    public void LoadData()
    {
        loadMapData.LoadMapLayout(level,savedData.levelData);
    }
    public LoadMapData GetLoadedData()
    {
        return loadMapData;
    }

}
