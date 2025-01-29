using TMPro;
using UnityEngine;

public class InWorldItem : MonoBehaviour
{
    [SerializeField]private Canvas canvas;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemCost;

    private CandlerShop parentScript;
    public int index;

    void Start()
    {
        parentScript = GetComponentInParent<CandlerShop>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemStockSO.ItemData itemData = parentScript.RecieveEnterCollisionData(collision, transform);
        itemName.text = itemData.itemName;
        itemCost.text = itemData.itemCost.ToString();
        ShowCanvas();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        parentScript.RecieveExitCollisionData(collision);
        HideCanvas();
    }

    private void ShowCanvas()
    {
        canvas.gameObject.SetActive(true);
    }
    private void HideCanvas()
    {
        canvas.gameObject.SetActive(false);
    }
}
