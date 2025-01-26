using UnityEngine;
using static ItemStockSO;

public class UIInventory : MonoBehaviour
{
    private PlayerInventory inventory;

    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;


    public void SetInventory(PlayerInventory inverntory)
    {
        this.inventory = inverntory;
        PlayerInventory.OnItemListChanged += OnInventoryUpdated;
        RefreshInventoryItems();
    }

    private void OnInventoryUpdated()
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        int xPos = 0;
        int yPos = 0;
        float itemSlotSizeX = 39;
        float itemSlotSizeY = 37.5f;

        foreach (ItemData item in inventory.GetInventory())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.GetComponent<UIItemHolder>().SetItem(item);
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.anchoredPosition = new Vector2(57 + (xPos * itemSlotSizeX), 44 + (yPos * itemSlotSizeY));

            xPos++;
            if (xPos > 3)
            {
                xPos = 0;
                yPos--;
            }
        }
    }
}
