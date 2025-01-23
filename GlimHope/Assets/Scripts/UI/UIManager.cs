using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private PauseUI pauseUI;


    private void Awake()
    {
        GameManager.onGameStateChange += HandleStateChange;
    }
    private void OnDestroy()
    {
        GameManager.onGameStateChange -= HandleStateChange;
    }

    private void HandleStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.MAIN_MENU)
        {
            LoadMainMenu();
            UnloadPauseMenu();
        }
        if(state == GameManager.GameState.LEVEL_1)
        {
            UnloadMainMenu();
            UnloadPauseMenu();
        }
        if (state == GameManager.GameState.PAUSE)
        {
            UnloadMainMenu();
            LoadPauseMenu();
        }
    }

    public void LoadMainMenu()
    {
        mainMenuUI.SetActive();
    }
    public void UnloadMainMenu()
    {
        mainMenuUI.SetUnactive();
    }
    public void LoadPauseMenu()
    {
        pauseUI.SetActive();
    }
    public void UnloadPauseMenu()
    {
        pauseUI.SetUnactive();
    }
}
