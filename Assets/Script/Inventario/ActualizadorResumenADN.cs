using UnityEngine;
using TMPro; // Asegúrate de tener TextMeshPro importado en tu proyecto

// Clase auxiliar para organizar las referencias en el Inspector
[System.Serializable]
public class UIRenglonADN
{
    public string nombreADN; // El nombre exacto del ADN (ej: "ADN Cazador Volador")
    public TextMeshProUGUI textoCantidad; // El campo de texto para mostrar la cantidad
}

public class ActualizadorResumenADN : MonoBehaviour
{
    // Una lista pública para que puedas asignar todos tus textos desde el Inspector
    public UIRenglonADN[] renglonesUI;

    // Nos suscribimos al evento del inventario cuando el objeto se activa
    private void OnEnable()
    {
        InventarioManagerPrueba.OnInventarioChanged += ActualizarTextos;
        // Llamamos una vez al inicio para asegurarnos de que los textos tengan el valor correcto
        ActualizarTextos();
    }

    // Nos desuscribimos del evento cuando el objeto se desactiva para evitar errores
    private void OnDisable()
    {
        InventarioManagerPrueba.OnInventarioChanged -= ActualizarTextos;
    }

    void ActualizarTextos()
    {
        // Verificación para evitar errores si el inventario aún no existe
        if (InventarioManagerPrueba.instancia == null) return;

        // Recorremos cada uno de los renglones que configuramos en el Inspector
        foreach (UIRenglonADN renglon in renglonesUI)
        {
            // Buscamos el slot correspondiente en el inventario real
            Slot slotDelInventario = System.Array.Find(
                InventarioManagerPrueba.instancia.slots,
                s => s.nombreADN == renglon.nombreADN
            );

            // Si encontramos el slot y el texto está asignado, actualizamos la UI
            if (slotDelInventario != null && renglon.textoCantidad != null)
            {
                renglon.textoCantidad.text = slotDelInventario.cantidad.ToString();
            }
        }
    }
}