using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
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
        mainMenuCanvas.SetActive(true);
    }
    public void SetUnactive()
    {
        mainMenuCanvas.SetActive(false);
    }

    public void StartGamePressed()
    {
        GameManager.instance.UpdateGameState(GameManager.GameState.START_GAME);
    }
}
