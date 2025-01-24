using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag=="Player")
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.NEXT_LEVEL);
        }
    }
}
