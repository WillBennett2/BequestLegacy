using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    public enum GameState {MAIN_MENU,PAUSE,UNPAUSE,LEVEL_1,LEVEL_2,LEVEL_3,LEVEL_4 }
    public static GameManager instance;
    public GameState gameState;
    private GameState priorState;
    public static event Action<GameState> onGameStateChange;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Level")]
    public MapGen mapGenerator;
    public int level;

    [Header("Saved Data")]
    [SerializeField] public SaveLevelDataSO savedData;
    private SaveMapData saveMapData;
    private LoadMapData loadMapData;


    private void Awake()
    {
        instance = this;

        savedData.levelData.Clear();
        loadMapData = new LoadMapData();
        saveMapData = new SaveMapData();
        saveMapData.SetSaveDataSO(savedData);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateGameState(GameState.MAIN_MENU);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateGameState(GameState.PAUSE);
        }
    }
    public void UpdateGameState(GameState newState)
    {
        GameState tempState = gameState;
        gameState = newState;

        switch (newState)
        {
            case GameState.MAIN_MENU:
                break;
            case GameState.PAUSE:
                priorState = tempState;
                break;
            case GameState.UNPAUSE:
                Debug.Log("unpause");
                gameState = priorState;
                break;
            case GameState.LEVEL_1:
                StartGame();
                break;
            case GameState.LEVEL_2:
                break;
            case GameState.LEVEL_3:
                break;
            case GameState.LEVEL_4:
                break;
        }

        onGameStateChange?.Invoke(newState);
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
