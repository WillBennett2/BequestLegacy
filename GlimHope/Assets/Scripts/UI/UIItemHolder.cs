using System;
using UnityEngine;
using static ItemStockSO;
using UnityEngine.EventSystems;

public class UIItemHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<ItemData> OnItemHovered;
    public static event Action OnStopItemHovered;

    public static Action OnInteractPause;
    public static Action OnInteractResume;

    [SerializeField] private ItemData m_itemData;
    [SerializeField] public bool m_isInventory;

    public ItemData GetItem()
    {
        return m_itemData;
    }
    public void SetItem(ItemData item)
    {
        m_itemData = item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            IsHovered();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            StopHovered();
        }
    }

    public void IsHovered()
    {
        OnItemHovered?.Invoke(m_itemData);

        OnInteractPause?.Invoke();
    }
    public void StopHovered()
    {

        OnStopItemHovered?.Invoke();

        OnInteractResume?.Invoke();
    }
}
