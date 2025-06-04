using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RecolectarADN : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public int quantity = 1;
    void OnMouseDown()
    {

        // Destruir el objeto recolectado
        Destroy(gameObject);
    }
    private void OnMouseDown()
    {
        Item item = new Item(itemName, icon, quantity);

        if (Inventory.Instance.AddItem(item))
        {
            FindObjectOfType<InventorySlotUI>().UpdateUI();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventario lleno");
        }
    }
}
