using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string ItemName;
    public Sprite Icon;
    public int quantity;
}
public class inventory : MonoBehaviour
{
    public static inventory Instance;

    public List<Item> Items = new List<Item>();
    public int maxSlots = 999;

    void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    public bool AddItem(Item Newitem)
    {
        if (Items.Count >= maxSlots) return false;
        Items.Add(Newitem);
        return true;
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}