using TMPro;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static ItemStockSO;
using static UnityEditor.Progress;

public class UIInventory : MonoBehaviour
{
    [SerializeField]private EntityStats entityStats;
    private PlayerInventory inventory;

    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;

    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private TMP_Text healthBurnText;
    [SerializeField] private TMP_Text spellDamageText;
    [SerializeField] private TMP_Text armourText;
    [SerializeField] private TMP_Text movementText;
    [SerializeField] private TMP_Text magicCritText;
    [SerializeField] private TMP_Text magicCritChanceText;
    [SerializeField] private TMP_Text attackSpeedText;

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
        UpdateText();
    }

    private void UpdateText()
    {
        maxHealthText.text = entityStats.maxHealth.ToString();
        healthBurnText.text = entityStats.healthBurn.ToString();
        spellDamageText.text = entityStats.spellDamage.ToString();
        armourText.text = entityStats.armour.ToString();
        movementText.text = entityStats.movementSpeed.ToString();
        magicCritText.text = entityStats.magicCritDamage.ToString();
        magicCritChanceText.text = entityStats.magicCritChance.ToString();
        attackSpeedText.text = entityStats.attackSpeed.ToString();

    }

}
