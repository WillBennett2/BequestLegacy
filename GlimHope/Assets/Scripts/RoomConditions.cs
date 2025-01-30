using System;
using UnityEngine;
using static RoomTypesSO;

public class RoomConditions : MonoBehaviour
{
    public static event Action<bool> OnRoomComplete;
    public static event Action<bool> OnEnterNewRoom;

    [SerializeField] public int index;
    [SerializeField] public RoomTypesSO.RoomData roomData;

    [SerializeField] private int enemyCount;

    private void Awake()
    {
        
    }
    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnRoomComplete?.Invoke(true);
        }
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
            OnRoomComplete?.Invoke(roomData.condition.completed);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("player in room");
        OnEnterNewRoom?.Invoke(roomData.condition.completed);
    }
}
