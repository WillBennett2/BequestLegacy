using UnityEngine;

public class Door : MonoBehaviour
{

    public int originRoomIndex;
    private Vector3 originalPos;
    [SerializeField]private GameObject visualGameObject;

    private void Awake()
    {
        RoomConditions.OnRoomComplete += OpenDoor;
        RoomConditions.OnEnterNewRoom += CloseDoor;

        originalPos = transform.position;
        visualGameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        RoomConditions.OnRoomComplete -= OpenDoor;
        RoomConditions.OnEnterNewRoom -= CloseDoor;
    }



    private void OpenDoor(bool conditionComplete)
    {
        visualGameObject.SetActive(false);
    }
    private void CloseDoor(bool conditionComplete)
    {
        if (!conditionComplete)
        {
            visualGameObject.SetActive(true);
        }
    }
}
