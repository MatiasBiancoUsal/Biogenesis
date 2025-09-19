using UnityEngine;
using UnityEngine.UI;
using TMPro;

// --- CLASE AUXILIAR PARA REFERENCIAS DE UI ---
[System.Serializable]
public class SlotUI
{
    public string nombreADN;
    public Image imagenSlot;
    public TextMeshProUGUI cantidadTexto;
}

public class InventarioUI : MonoBehaviour
{
    public GameObject panelInventario;
    public SlotUI[] slotsVisuales;

    // Al activarse, nos suscribimos al evento
    private void OnEnable()
    {
        Debug.Log("<color=yellow>SUSCRIPCI�N:</color> InventarioUI se ha activado y suscrito al evento OnInventarioChanged.");
        InventarioManagerPrueba.OnInventarioChanged += ActualizarUI;
        // Actualizamos una vez por si ya hab�a datos
        ActualizarUI();
    }

    // Al desactivarse, nos damos de baja
    private void OnDisable()
    {
        Debug.Log("<color=orange>DESUSCRIPCI�N:</color> InventarioUI se ha desactivado y quitado la suscripci�n.");
        InventarioManagerPrueba.OnInventarioChanged -= ActualizarUI;
    }

    // --- DENTRO DE InventarioUI.cs ---

    void ActualizarUI()
    {
        if (InventarioManagerPrueba.instancia == null) return;

        foreach (SlotUI slotV in slotsVisuales)
        {
            // 1. Buscamos el �cono en la nueva base de datos del manager
            Sprite iconoCorrecto = InventarioManagerPrueba.instancia.GetIconoPorNombre(slotV.nombreADN);
            slotV.imagenSlot.sprite = iconoCorrecto; // Asignamos el �cono SIEMPRE

            // 2. Buscamos los datos de la partida (cantidad)
            Slot slotDato = null;
            foreach (Slot s in InventarioManagerPrueba.instancia.slots)
            {
                if (s.nombreADN == slotV.nombreADN)
                {
                    slotDato = s;
                    break;
                }
            }

            // 3. Actualizamos la cantidad y el color
            if (slotDato != null && slotDato.cantidad > 0)
            {
                // Si tenemos el item, mostramos su cantidad y color normal
                slotV.cantidadTexto.text = slotDato.cantidad.ToString();
                slotV.imagenSlot.color = Color.white;
            }
            else
            {
                // Si no tenemos el item, mostramos "0" y oscurecemos el �cono
                slotV.cantidadTexto.text = "0";
                slotV.imagenSlot.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            }
        }
    }
}