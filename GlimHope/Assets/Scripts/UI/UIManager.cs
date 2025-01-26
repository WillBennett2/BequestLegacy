using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private PauseUI pauseUI;

    [SerializeField] private GameObject inventoryUI;
    private bool inventoryUIActive = false;


    private void Awake()
    {
        GameManager.onGameStateChange += HandleStateChange;
        PlayerController.OnShowInventory += ToggleInventoryUIActive;
    }
    private void OnDestroy()
    {
        GameManager.onGameStateChange -= HandleStateChange;
        PlayerController.OnShowInventory -= ToggleInventoryUIActive;
    }

    private void HandleStateChange(GameManager.GameState state)
    {
        if (state == GameManager.GameState.MAIN_MENU)
        {
            LoadMainMenu();
            UnloadPauseMenu();
        }
        if(state == GameManager.GameState.START_GAME)
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

    private void ToggleInventoryUIActive()
    {
        if(inventoryUIActive == false)
        {
            ShowInventory();
            inventoryUIActive = true;
        }
        else
        {
            HideInventory();
            inventoryUIActive = false;
        }
    }

    public void ShowInventory()
    {
        inventoryUI.SetActive(true);
    }
    public void HideInventory()
    {
        inventoryUI.SetActive(false);
    }
}
