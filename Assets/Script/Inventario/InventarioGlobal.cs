using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioGlobal : MonoBehaviour
{
    public static InventarioGlobal Instance;

    [System.Serializable]
    public class ADNItem
    {
        public string nombre;
        public int cantidad;
    }

    public List<ADNItem> itemsADN = new List<ADNItem>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // Ya existe, destruir duplicado
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Inicializamos si está vacío
        if (itemsADN.Count == 0)
        {
            itemsADN.Add(new ADNItem { nombre = "Alimaña", cantidad = 0 });
            itemsADN.Add(new ADNItem { nombre = "Araña", cantidad = 0 });
            itemsADN.Add(new ADNItem { nombre = "Cazador", cantidad = 0 });
            itemsADN.Add(new ADNItem { nombre = "Mutante", cantidad = 0 });
        }
    }

    public void AgregarADN(string nombre, int cantidad)
    {
        ADNItem item = itemsADN.Find(i => i.nombre == nombre);
        if (item != null)
        {
            item.cantidad += cantidad;
            Debug.Log("Agregado ADN: " + nombre + " x" + cantidad);
        }
    }

    public int ObtenerCantidad(string nombre)
    {
        ADNItem item = itemsADN.Find(i => i.nombre == nombre);
        return item != null ? item.cantidad : 0;
    }
}
