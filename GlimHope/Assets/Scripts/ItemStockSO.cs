using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStockSO", menuName = "Scriptable Objects/ItemStockSO")]
public class ItemStockSO : ScriptableObject
{
    [SerializeField] public List<ItemData> itemTypes;



    [Serializable]
    public struct ItemData
    {
        [SerializeField][Tooltip("This should be a percentage out of 100")] public int spawnWeight;
        [SerializeField] public GameObject item;
        [SerializeField] public int itemCost;
    }
}
