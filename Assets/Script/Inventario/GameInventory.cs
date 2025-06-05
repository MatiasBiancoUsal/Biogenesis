using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
namespace Game.InventorySystem
{
    public class GameInventory : MonoBehaviour
    {
        public static GameInventory Instance;
        public List<Item> Items = new List<Item>();
        public int maxSlots = 999;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
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

        public class InventorySlot
        {
            public Item item;
            public int amount;

            public InventorySlot(Item item, int amount)
            {
                this.item = item;
                this.amount = amount;
            }
        }
        public List<InventorySlot> items = new List<InventorySlot>();

        public InventorySlot GetItem(Item item)
        {
            return items.Find(i => i.item == item && i.amount > 0);
        }

    }
}
