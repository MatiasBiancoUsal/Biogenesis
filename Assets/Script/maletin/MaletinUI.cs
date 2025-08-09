using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaletinUI : MonoBehaviour
{
    public GameObject maletinPanel; // Tu panel del malet�n
    public Transform panelPociones; // Donde van los iconos
    public GameObject prefabIcono;  // Prefab del icono de poci�n

    public void ToggleMaletinInventory()
    {
        bool isActive = maletinPanel.activeSelf;
        maletinPanel.SetActive(!isActive);

        // Si se abri�, actualizamos la UI
        if (!isActive)
        {
            ActualizarUI();
        }
    }

    void ActualizarUI()
    {
        // Limpiar lo que hubiera antes
        foreach (Transform hijo in panelPociones)
        {
            Destroy(hijo.gameObject);
        }

        // Mostrar todas las pociones guardadas en el MaletinManager
        foreach (var pocion in MaletinManager.instancia.pociones)
        {
            GameObject nuevaPocionUI = Instantiate(prefabIcono, panelPociones);
            nuevaPocionUI.GetComponent<Image>().sprite = pocion.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
