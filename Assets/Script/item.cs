using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameInventory;

namespace GameInventory
{
    public class ItemCombiner : MonoBehaviour
    {
        public Item ADNCazadorvolador;
        public Item ADNAlimañaBiotecnologica;
        public Item ADNArañaMutante;
        public Item ADNMutanteRadiactivo;
        GameObject CriaturaExperimento;

        [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

        internal static class InventoryEvents
        {
            public static Action OnInventoryChanged = delegate { };
        }
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

                // Agrega criatura experimento
                inventory.AddItem(CriaturaExperimento);

                Debug.Log("¡Has creado Criatura Experimento!");
            }
            else
            {
                Debug.Log("Faltan materiales para crear Criatura Experimento.");
            }


            InventoryEvents.OnInventoryChanged.Invoke();
        }
    }

    internal class InventoryEvents
    {
        public static object OnInventoryChanged { get; internal set; }
    }
}