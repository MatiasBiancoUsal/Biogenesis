using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerADN : MonoBehaviour
{
    public string nombreADN;

    private void OnMouseDown()
    {
        if (InventarioGlobal.Instance != null)
        {
            InventarioGlobal.Instance.AgregarADN(nombreADN, 1);
            Destroy(gameObject); // Elimina el ítem de la escena
        }
        else
        {
            Debug.LogWarning("InventarioGlobal no está disponible.");
        }
    }
}
