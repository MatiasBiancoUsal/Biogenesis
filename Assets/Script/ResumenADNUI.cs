using UnityEngine;
using TMPro;

public class ResumenADNUI : MonoBehaviour
{
    public TextMeshProUGUI[] textosCantidad; // Uno por cada ADN, ordenados igual que en slots del inventario

    void Update()
    {
        if (InventarioGlobal.Instance == null)
            return;

        textosCantidad[0].text = InventarioGlobal.Instance.ObtenerCantidad("Alimaña").ToString();
        textosCantidad[1].text = InventarioGlobal.Instance.ObtenerCantidad("Araña").ToString();
        textosCantidad[2].text = InventarioGlobal.Instance.ObtenerCantidad("Cazador").ToString();
        textosCantidad[3].text = InventarioGlobal.Instance.ObtenerCantidad("Mutante").ToString();
    }
}
