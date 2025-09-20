using UnityEngine;
using UnityEngine.UI;
using TMPro;

// --- CLASE AUXILIAR ---
[System.Serializable]
public class SlotUI
{
    public string nombreADN;
    public Image imagenFondoSlot;
    public Image imagenIconoItem;
    public TextMeshProUGUI cantidadTexto;
}

public class InventarioUI : MonoBehaviour
{
    public GameObject panelInventario;
    public SlotUI[] slotsVisuales;

    [Header("Componentes Adicionales")]
    public GameObject botonCrearCriatura;

    private void OnEnable()
    {
        InventarioManagerPrueba.OnInventarioChanged += ActualizarUI;
        ActualizarUI(); // Actualiza al abrir el inventario
    }

    private void OnDisable()
    {
        InventarioManagerPrueba.OnInventarioChanged -= ActualizarUI;
    }

    void ActualizarUI()
    {
        if (InventarioManagerPrueba.instancia == null) return;

        // Actualiza cada slot visual
        foreach (SlotUI slotV in slotsVisuales)
        {
            Sprite iconoCorrecto = InventarioManagerPrueba.instancia.GetIconoPorNombre(slotV.nombreADN);
            Slot slotDato = System.Array.Find(InventarioManagerPrueba.instancia.slots, s => s.nombreADN == slotV.nombreADN);

            if (slotDato != null && slotDato.cantidad > 0)
            {
                slotV.imagenIconoItem.sprite = iconoCorrecto;
                slotV.imagenIconoItem.enabled = true;
                slotV.cantidadTexto.text = slotDato.cantidad.ToString();
                slotV.imagenFondoSlot.color = Color.white;
            }
            else
            {
                slotV.imagenIconoItem.enabled = false;
                slotV.cantidadTexto.text = "0";
                slotV.imagenFondoSlot.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            }
        }

        // L�gica para mostrar/ocultar el bot�n de crear criatura
        if (InventarioManagerPrueba.instancia.TodosLosADNRecolectados() && !InventarioManagerPrueba.instancia.criaturaCreada)
        {
            if (botonCrearCriatura != null) botonCrearCriatura.SetActive(true);
        }
        else
        {
            if (botonCrearCriatura != null) botonCrearCriatura.SetActive(false);
        }
    }
}