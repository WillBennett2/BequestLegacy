using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static event Action<int> OnPurchaseItem;

    [SerializeField]private int totalMoney;

    private void Awake()
    {
        PlayerController.OnPurchaseItem += HandlePlayerPurchaseInput;
        CandlerShop.OnCostOfItem += HandleCostOfItem;
    }
    private void OnDestroy()
    {
        PlayerController.OnPurchaseItem -= HandlePlayerPurchaseInput;
        CandlerShop.OnCostOfItem -= HandleCostOfItem;
    }

    private void HandlePlayerPurchaseInput()
    {
        OnPurchaseItem?.Invoke(totalMoney);
    }
    private void HandleCostOfItem(int itemCost)
    {
        totalMoney -= itemCost;
    }
}
