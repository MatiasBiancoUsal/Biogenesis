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
        foreach (var pocion in MaletinManager.instancia.ObtenerPociones())
        {
            GameObject nuevaPocionUI = null;

            if (pocion.CompareTag("PocionVida"))
            {
                nuevaPocionUI = Instantiate(prefabIconoVida, panelPocionesVida);
            }
            else if (pocion.CompareTag("PocionMejora"))
            {
                nuevaPocionUI = Instantiate(prefabIconoMejora, panelPocionesMejora);
            }

            if (nuevaPocionUI != null)
            {
                Image img = nuevaPocionUI.GetComponent<Image>();

                // Primero probamos si la poción original es UI
                Image imgOriginal = pocion.GetComponent<Image>();
                if (imgOriginal != null)
                {
                    img.sprite = imgOriginal.sprite;
                }
                else
                {
                    // Si no es UI, probamos si es un objeto del mundo con SpriteRenderer
                    SpriteRenderer sr = pocion.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        img.sprite = sr.sprite;
                    }
                }
            }
        }
    }
}
