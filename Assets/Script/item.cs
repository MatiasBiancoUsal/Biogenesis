using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Item
    {
        public string itemName;
        public Sprite icon;
        public int quantity;

        public Item(string name, Sprite icon, int qty = 1)
        {
            this.itemName = name;
            this.icon = icon;
            this.quantity = qty;
        }
    }

    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

    public class ItemCombiner : MonoBehaviour
    {
        public Item ADNCazadorvolador;
        public Item ADNAlimañaBiotecnologica;
        public Item ADNArañaMutante;
        public Item ADNMutanteRadiactivo;
        GameObject CriaturaExperimento;

        public void TryCombine()
        {
            Inventory inventory = Inventory.Instance;

            // Verifica si tiene todos los ítems
            if (inventory.HasItem(ADNCazadorvolador) &&
                inventory.HasItem(ADNAlimañaBiotecnologica) &&
                inventory.HasItem(ADNArañaMutante) &&
                inventory.HasItem(ADNMutanteRadiactivo))
            {
                // Remueve 1 unidad de cada ítem
                inventory.RemoveItem(ADNCazadorvolador, 1);
                inventory.RemoveItem(ADNAlimañaBiotecnologica, 1);
                inventory.RemoveItem(ADNArañaMutante, 1);
                inventory.RemoveItem(ADNMutanteRadiactivo, 1);

                // Agrega Mob 1
                inventory.AddItem(CriaturaExperimento);

                Debug.Log("¡Has creado Criatura Experimento!");
            }
            else
            {
                Debug.Log("Faltan materiales para crear Criatura Experimento.");
            }

            InventoryEvents.OnInventoryChanged?.Invoke();
        }
    }
}