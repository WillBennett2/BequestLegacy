using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState {MAIN_MENU,PAUSE,LEVEL_1,LEVEL_2,LEVEL_3,LEVEL_4 }
    private static GameManager instance;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Level")]
    public MapGen mapGenerator;
    public int level;

    [Header("Saved Data")]
    [SerializeField] public SaveLevelDataSO savedData;
    private SaveMapData saveMapData;
    private LoadMapData loadMapData;


    public static GameManager Instance
    {
        get
        {
            if (GameManager.instance==null)
            {
                DontDestroyOnLoad(GameManager.instance);
                GameManager.instance = new GameManager();
            }
            return GameManager.instance; 
        }
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }








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
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            mapGenerator.GenerateMap();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            mapGenerator.LoadMap();
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            level++;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            level--;
        }
    }

    private void StartGame()
    {
        //load scene

        //generate map
        mapGenerator.GenerateMap();
        //place player
        GameObject player = Instantiate(playerPrefab,mapGenerator.GetStartTilePos(),Quaternion.identity);


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
