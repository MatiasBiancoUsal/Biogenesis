using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class RecolectarADN : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public int quantity = 1;

    void OnMouseDown()
    {
        if (InventarioGlobal.Instance != null)
        {
            InventarioGlobal.Instance.AgregarADN(itemName, quantity);
        }
        else
        {
            Debug.LogWarning("InventarioGlobal no encontrado.");
        }

        Destroy(gameObject);
    }


}
