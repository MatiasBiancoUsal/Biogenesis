using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
namespace Game.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;
        public List<Item> Items = new List<Item>();
        public int maxSlots = 999;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public bool AddItem(Item newItem)
        {
            foreach (Item item in Items)
            {
                if (item.itemName == newItem.itemName)
                {
                    item.quantity += newItem.quantity;
                    return true;
                }
            }

            if (Items.Count >= maxSlots) return false;

            Items.Add(new Item(newItem.itemName, newItem.icon, newItem.quantity)); // Clonar
            return true;
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        public class Item
        {
            public string itemName;
            public UnityEngine.Sprite icon;
            public int quantity;

            public Item(string name, Sprite icon, int qty = 1)
            {
                this.itemName = name;
                this.icon = icon;
                this.quantity = qty;
            }
        }

        public bool HasItem(Item item)
        {
            return Item.Any(i => i.item == item && i.amount > 0);
        }
    }
}
