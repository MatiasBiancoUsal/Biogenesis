using UnityEngine;

public class AbrirInventarioDesdeResumen : MonoBehaviour
{
    public GameObject inventarioPanel; // Debe estar asignado en el Inspector

    public void AlternarInventario()
    {
        if (inventarioPanel != null)
        {
            inventarioPanel.SetActive(!inventarioPanel.activeSelf);
            Debug.Log("Inventario alternado"); // Para verificar que se ejecuta
        }
        else
        {
            Debug.LogWarning("InventarioPanel no está asignado.");
        }
    }
}
