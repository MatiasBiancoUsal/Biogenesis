using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventorySlotUI : MonoBehaviour
{
    public class InventorySlotUI : MonoBehaviour
    {
        public GameObject inventoryPanel;
        public List<InventorySlot> slots = new List<InventorySlot>();

        void Start()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < inventory.Instance.Items.Count)
                {
                    slots[i].AddItem(inventory.Instance.Items[i]);
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }
        }
    }
}


