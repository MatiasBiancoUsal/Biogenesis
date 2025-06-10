using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirMesaDePociones : MonoBehaviour
{
    public GameObject panelMesaPociones;

    void OnMouseDown()
    {
        if (panelMesaPociones != null)
        {
            panelMesaPociones.SetActive(true);
        }
    }
}
