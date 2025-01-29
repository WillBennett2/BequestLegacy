using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class CandlerShop : MonoBehaviour
{
    public static event Action<int> OnCostOfItem;
    public static event Action<ItemStockSO.ItemData> OnGainedItem;

    [SerializeField] private ItemStockSO itemStockData;
    [SerializeField] private List<GameObject> items;
    [SerializeField] private List<Transform> itemLocations;

    private int itemToPurchaseIndex = -1;


    // offer 3 different items
    // items are randomly assigned from SO?

    private void Awake()
    {
        PlayerInventory.OnPurchaseItem += BuyItem;
        StockShop();
    }
    private void OnDestroy()
    {
        PlayerInventory.OnPurchaseItem -= BuyItem;
    }

    private void StockShop()
    {
        for (int i = 0; i < itemLocations.Count; i++)
        {
            int randomItemIndex = GetRandomItemIndex();
            GameObject item = Instantiate(itemStockData.itemTypes[randomItemIndex].item, itemLocations[i].position, Quaternion.identity, transform);
            item.GetComponent<InWorldItem>().index = i;
            items.Add(item);
        }
    }
    
    private int GetRandomItemIndex()
    {
        int totalWeight = itemStockData.itemTypes.Sum(x => x.spawnWeight);
        int wantedWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;

        for (int i = 0; i < itemStockData.itemTypes.Count; i++)
        {
            currentWeight += itemStockData.itemTypes[i].spawnWeight;
            if (currentWeight >= wantedWeight)
            {
                return i;
            }
        }

        return -1;
    }

    public ItemStockSO.ItemData RecieveEnterCollisionData(Collider2D hit, Transform itemHit)
    {
        for (int i = 0; i < itemLocations.Count; i++)
        {
            if (itemHit.transform.position == itemLocations[i].position)
            {
                itemToPurchaseIndex = itemHit.GetComponent<InWorldItem>().index;
            }
        }
        return itemStockData.itemTypes[itemToPurchaseIndex];

    }
    public void RecieveExitCollisionData(Collider2D hit)
    {
        itemToPurchaseIndex = -1;
    }

    private void BuyItem(int totalMoney)
    {
        if (itemToPurchaseIndex == -1)
        {
            return;
        }
        //check funds
        if (itemStockData.itemTypes[itemToPurchaseIndex].itemCost < totalMoney && items[itemToPurchaseIndex].activeInHierarchy)
        {
            Debug.Log("Player has purchased " + items[itemToPurchaseIndex].name+" for a cost of "+ itemStockData.itemTypes[itemToPurchaseIndex].itemCost);
            OnCostOfItem(itemStockData.itemTypes[itemToPurchaseIndex].itemCost);
            OnGainedItem.Invoke(itemStockData.itemTypes[itemToPurchaseIndex]);
            items[itemToPurchaseIndex].SetActive(false);
        }
    }
}
