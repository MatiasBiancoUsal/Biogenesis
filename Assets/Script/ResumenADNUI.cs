using UnityEngine;
using TMPro;

public class ResumenADNUI : MonoBehaviour
{
    public TextMeshProUGUI[] textosCantidad; // Uno por cada ADN, ordenados igual que en slots del inventario

    void Update()
    {
        if (InventarioManager.instancia == null || InventarioManager.instancia.slots == null)
            return;

        for (int i = 0; i < textosCantidad.Length; i++)
        {
            int cantidad = InventarioManager.instancia.slots[i].cantidad;
            textosCantidad[i].text = cantidad.ToString();
        }
    }
}
