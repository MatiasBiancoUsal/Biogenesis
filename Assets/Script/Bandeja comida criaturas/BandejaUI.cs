using UnityEngine;
using UnityEngine.UI;

public class BandejaUI : MonoBehaviour
{
    [Header("Datos")]
    public InventarioComida inventario; // arrastrá el asset aquí

    [Header("UI")]
    public GameObject panelBandeja;     // el Panel que se muestra/oculta
    public Image[] slots;               // 3 imágenes de los slots

    void OnEnable()
    {
        if (inventario != null)
            inventario.OnCambio += ActualizarUI;

        ActualizarUI();
    }

    void OnDisable()
    {
        if (inventario != null)
            inventario.OnCambio -= ActualizarUI;
    }

    public void ToggleBandeja()
    {
        if (panelBandeja == null) return;
        panelBandeja.SetActive(!panelBandeja.activeSelf);
        if (panelBandeja.activeSelf) ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (slots == null || inventario == null)
        {
            Debug.LogWarning("Slots o inventario son null en ActualizarUI");
            return;
        }


        Debug.Log(">> Actualizando UI, comidas en inventario: " + inventario.comidas.Count);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventario.comidas.Count && inventario.comidas[i] != null)
            {
                slots[i].sprite = inventario.comidas[i];
                slots[i].enabled = true;
                Debug.Log($"Slot {i} actualizado con sprite: {inventario.comidas[i].name}");
            }
            else
            {
                slots[i].sprite = null;
                slots[i].enabled = false;
                Debug.Log($"Slot {i} vacío");
            }
        }

    }
}
