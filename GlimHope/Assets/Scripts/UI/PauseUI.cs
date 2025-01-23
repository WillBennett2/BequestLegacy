using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActive()
    {
        pauseMenuCanvas.SetActive(true);
    }
    public void SetUnactive()
    {
        pauseMenuCanvas.SetActive(false);
    }

    //public void PauseGamePressed()
    //{
    //    GameManager.instance.UpdateGameState(GameManager.GameState.PAUSE);
    //}
    public void UnpauseGamePressed()
    {
        SetUnactive();
        GameManager.instance.UpdateGameState(GameManager.GameState.UNPAUSE);
    }
    public void SettingsPressed()
    {
        //GameManager.instance.UpdateGameState(GameManager.GameState.UNPAUSE);
    }
    public void ExitPressed()
    {
        //GameManager.instance.UpdateGameState(GameManager.GameState.UNPAUSE);
    }
}
