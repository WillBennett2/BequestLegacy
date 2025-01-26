using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemStockSO;

public class UIHoverItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_itemDetails = new List<GameObject>();
    [SerializeField] private GameObject m_itemDetailContainer;

    [SerializeField] private PlayerInventory m_inventory;

    private void ShowItemDetails(ItemData item)
    {

        m_itemDetailContainer.SetActive(true);
        int index = 0;
        m_itemDetails[index].GetComponentInChildren<Text>().text = item.itemName.ToString();
        m_itemDetails[index].SetActive(true);
        index++;
        if (0 != item.healthChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Heath          " + item.healthChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.healthBurnChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Health Burn    " + item.healthBurnChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.spellDamageChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Spell Damage   " + item.spellDamageChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.armourChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Armour         " + item.armourChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.movementSpeedChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Movement Speed " + item.movementSpeedChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.magicCritChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Magic Crit     " + item.magicCritChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.magicCritChanceChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Magic Crit %   " + item.magicCritChanceChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }
        if (0 != item.attackSpeedChange)
        {
            m_itemDetails[index].GetComponentInChildren<Text>().text = "Attack Speed   " + item.attackSpeedChange.ToString();
            m_itemDetails[index].SetActive(true);
            index++;
        }

    }
    private void HideItemDetails()
    {
        foreach (GameObject itemDetail in m_itemDetails)
        {
            itemDetail.SetActive(false);
        }
        m_itemDetailContainer.SetActive(false);
    }

    private void OnEnable()
    {
        UIItemHolder.OnItemHovered += ShowItemDetails;
        UIItemHolder.OnStopItemHovered += HideItemDetails;
    }
    private void OnDisable()
    {
        UIItemHolder.OnItemHovered -= ShowItemDetails;
        UIItemHolder.OnStopItemHovered -= HideItemDetails;
    }
}
