using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private bool roomLock = false;

    private Vector3 targetPosition;
    [SerializeField] private Transform player;
    private Vector2 roomPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer)
            targetPosition = player.position;
        else
            targetPosition = roomPosition;

        transform.position = Vector3.MoveTowards(transform.position,new Vector3(targetPosition.x, targetPosition.y, transform.position.z), 1);


    }

    private void LockToRoomPosition(Vector2 roomPos)
    {
        followPlayer = false;
        roomLock = true;
        roomPosition = roomPos;
    }
     private void LockToPlayerPostion()
    {
        followPlayer = true;
        roomLock = false;
    }

}
