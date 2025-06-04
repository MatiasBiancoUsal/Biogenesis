using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using InvItem = Inventory.Item;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text amountText;

    public void SetSlot(InvItem item, int amount)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
        amountText.text = amount > 1 ? amount.ToString() : "";
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = "";
    }

}
