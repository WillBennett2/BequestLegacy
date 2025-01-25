using UnityEngine;

public class InWorldItem : MonoBehaviour
{
    private CandlerShop parentScript;
    public int index;
    void Start()
    {
        parentScript = GetComponentInParent<CandlerShop>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentScript.RecieveEnterCollisionData(collision, transform);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        parentScript.RecieveExitCollisionData(collision);
    }
}
