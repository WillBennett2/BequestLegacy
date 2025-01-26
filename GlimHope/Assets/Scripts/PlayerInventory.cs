using System;
using System.Collections.Generic;
using UnityEngine;
using static ItemStockSO;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<int> OnPurchaseItem;
    public static event Action OnItemListChanged;

    [SerializeField] private UIInventory inventoryUI;
    [SerializeField] private int inventorySize;
    [SerializeField] private List<ItemData> m_itemsInventory = new List<ItemData>();

    [SerializeField]private int totalMoney;

    private void Awake()
    {
        PlayerController.OnPurchaseItem += HandlePlayerPurchaseInput;
        CandlerShop.OnCostOfItem += HandleCostOfItem;
        CandlerShop.OnGainedItem += AddItem;
        inventoryUI.SetInventory(this);
    }
    private void OnDestroy()
    {
        PlayerController.OnPurchaseItem -= HandlePlayerPurchaseInput;
        CandlerShop.OnCostOfItem -= HandleCostOfItem;
        CandlerShop.OnGainedItem -= AddItem;
    }

    private void HandlePlayerPurchaseInput()
    {
        OnPurchaseItem?.Invoke(totalMoney);
    }
    private void HandleCostOfItem(int itemCost)
    {
        totalMoney -= itemCost;
    }

    public void AddItem(ItemData newItem)
    {
        Debug.Log("item added");
        m_itemsInventory.Insert(0, newItem);
        if (inventorySize < m_itemsInventory.Count)
        {
            Debug.Log("bye bye item");
            m_itemsInventory.RemoveAt(m_itemsInventory.Count - 1);
        }
        ApplyItemStats();
        OnItemListChanged?.Invoke();
    }

    private void ApplyItemStats()
    { 
    }

    public List<ItemData> GetInventory()
    {
        return m_itemsInventory;
    }

}
