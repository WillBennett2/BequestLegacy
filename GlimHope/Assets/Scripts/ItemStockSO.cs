using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStockSO", menuName = "Scriptable Objects/ItemStockSO")]
public class ItemStockSO : ScriptableObject
{
    public enum ItemTypes
    {

    }
    [SerializeField] public List<ItemData> itemTypes;

    [Serializable]
    public struct ItemData
    {
        [SerializeField] public string itemName;
        [SerializeField][Tooltip("This should be a percentage out of 100")] public int spawnWeight;
        [SerializeField] public GameObject item;
        [SerializeField] public int itemCost;

        public int healthChange;
        public int healthBurnChange;
        public int spellDamageChange;
        public int armourChange;
        public int movementSpeedChange;
        public int magicCritChange;
        public int magicCritChanceChange;
        public int attackSpeedChange;
    }
}
