using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class InventorySlotUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < Inventory.Instance.Items.Count)
            {
                Item item = Inventory.Instance.Items[i];
                slots[i].SetSlot(item, item.quantity);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

}
