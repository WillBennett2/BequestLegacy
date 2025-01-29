using System;
using System.Collections.Generic;
using UnityEngine;
using static ItemStockSO;
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<int> OnPurchaseItem;
    public static event Action OnItemListChanged;

    private EntityStats entityStats;


    [SerializeField] private UIInventory inventoryUI;
    [SerializeField] private int inventorySize;
    [SerializeField] private List<ItemData> m_itemsInventory = new List<ItemData>();

    [SerializeField]private int totalMoney;

    private void Awake()
    {
        PlayerController.OnPurchaseItem += HandlePlayerPurchaseInput;
        CandlerShop.OnCostOfItem += HandleCostOfItem;
        CandlerShop.OnGainedItem += AddItem;
        entityStats = GetComponent<EntityStats>();
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
        ApplyItemStats(newItem);
        OnItemListChanged?.Invoke();
    }

    private void ApplyItemStats(ItemData newItem)
    {
        if (0 != newItem.maxHealthChange)
        {
            entityStats.UpdateMaxHealthValue(newItem.maxHealthChange);
        }
        if (0 != newItem.healthBurnChange)
        {
            entityStats.UpdateHealthBurnValue(newItem.healthBurnChange);
        }
        if (0 != newItem.spellDamageChange)
        {
            entityStats.UpdateSpellDamageValue(newItem.spellDamageChange);
        }
        if (0 != newItem.armourChange)
        {
            entityStats.UpdateArmourValue(newItem.armourChange);
        }
        if (0 != newItem.movementSpeedChange)
        {
            entityStats.UpdateMovementSpeedValue(newItem.movementSpeedChange);
        }
        if (0 != newItem.magicCritChange)
        {
            entityStats.UpdateMagicCritDamageValue(newItem.magicCritChange);
        }
        if (0 != newItem.magicCritChanceChange)
        {
            entityStats.UpdateMagicCritChanceValue(newItem.magicCritChanceChange);
        }
        if (0 != newItem.attackSpeedChange)
        {
            entityStats.UpdateAttackSpeedValue(newItem.attackSpeedChange);
        }
    }

    public List<ItemData> GetInventory()
    {
        return m_itemsInventory;
    }

}
