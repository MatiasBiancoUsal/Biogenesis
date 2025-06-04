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
        public Item ADNAlima�aBiotecnologica;
        public Item ADNAra�aMutante;
        public Item ADNMutanteRadiactivo;
        GameObject CriaturaExperimento;

        public void TryCombine()
        {
            Inventory inventory = Inventory.Instance;

            // Verifica si tiene todos los �tems
            if (inventory.HasItem(ADNCazadorvolador) &&
                inventory.HasItem(ADNAlima�aBiotecnologica) &&
                inventory.HasItem(ADNAra�aMutante) &&
                inventory.HasItem(ADNMutanteRadiactivo))
            {
                // Remueve 1 unidad de cada �tem
                inventory.RemoveItem(ADNCazadorvolador, 1);
                inventory.RemoveItem(ADNAlima�aBiotecnologica, 1);
                inventory.RemoveItem(ADNAra�aMutante, 1);
                inventory.RemoveItem(ADNMutanteRadiactivo, 1);

                // Agrega Mob 1
                inventory.AddItem(CriaturaExperimento);

                Debug.Log("�Has creado Criatura Experimento!");
            }
            else
            {
                Debug.Log("Faltan materiales para crear Criatura Experimento.");
            }

            InventoryEvents.OnInventoryChanged?.Invoke();
        }
    }
}