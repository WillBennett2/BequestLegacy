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
        //if (0 < item.m_damageIncrease)
        //{
        //    m_itemDetails[index].GetComponentInChildren<Text>().text = "Strength        " + item.m_damageIncrease.ToString();
        //    m_itemDetails[index].SetActive(true);
        //    index++;
        //}
        //if (0 < item.m_attackSpeedIncrease)
        //{
        //    m_itemDetails[index].GetComponentInChildren<Text>().text = "Attack Speed    " + item.m_attackSpeedIncrease.ToString();
        //    m_itemDetails[index].SetActive(true);
        //    index++;
        //}
        //if (0 < item.m_ACIncrease)
        //{
        //    m_itemDetails[index].GetComponentInChildren<Text>().text = "Defence         " + item.m_ACIncrease.ToString();
        //    m_itemDetails[index].SetActive(true);
        //    index++;
        //}
        //if (0 < item.m_healthIncrease)
        //{
        //    m_itemDetails[index].GetComponentInChildren<Text>().text = "Max HP          " + item.m_healthIncrease.ToString();
        //    m_itemDetails[index].SetActive(true);
        //    index++;
        //}
        //if (0 < item.m_evasion)
        //{
        //    m_itemDetails[index].GetComponentInChildren<Text>().text = "Evasion         " + item.m_evasion.ToString();
        //    m_itemDetails[index].SetActive(true);
        //    index++;
        //}
        //if (0 < item.m_vamperism)
        //{
        //    m_equipedDetails[index].GetComponentInChildren<Text>().text = "Vamperism         " + item.m_vamperism.ToString();
        //    m_equipedDetails[index].SetActive(true);
        //    index++;
        //}
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
