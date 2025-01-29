using UnityEngine;

public class RoomConditions : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] public RoomTypesSO.RoomData roomData;

    [SerializeField] private int enemyCount;

    private void Awake()
    {
        
    }
    private void OnDestroy()
    {
        
    }


    private void HandleEnemyDeath(int roomIndex)
    {
        if(roomIndex != index)
        {
            return;
        }
        enemyCount++;
        if (enemyCount==roomData.condition.completionValue)
        {
            roomData.condition.completed = true;
        }
    }
}
