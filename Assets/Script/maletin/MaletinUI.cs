using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaletinUI : MonoBehaviour
{
    public GameObject maletinPanel; // Panel del maletín
    public Transform panelPocionesVida;    // Donde van las pociones de vida
    public Transform panelPocionesMejora;  // Donde van las pociones de mejora
    public GameObject prefabIconoVida;
    public GameObject prefabIconoMejora;

    

    public void ToggleMaletinInventory()
    {
        bool isActive = maletinPanel.activeSelf;
        maletinPanel.SetActive(!isActive);

        // Si se abrió, actualizamos la UI
        if (!isActive)
        {
            ActualizarUI();
        }
    }

    private void OnEnable()
    {
        ActualizarUI();
    }

   
    void ActualizarUI()
    {
        if (MaletinManager.instancia == null)
            return; // No hay maletín cargado, no hacemos nada

        // Limpiar ambos paneles
        foreach (Transform hijo in panelPocionesVida)
        {
            Destroy(hijo.gameObject);
        }
        foreach (Transform hijo in panelPocionesMejora)
        {
            Destroy(hijo.gameObject);
        }

        // Mostrar solo las pociones que realmente están en el maletín
        if (MaletinManager.instancia.tienePocionVida())
        {
            Instantiate(prefabIconoVida, panelPocionesVida);
        }

        if (MaletinManager.instancia.tienePocionMejora())
        {
            Instantiate(prefabIconoMejora, panelPocionesMejora);
        }
    }
}
