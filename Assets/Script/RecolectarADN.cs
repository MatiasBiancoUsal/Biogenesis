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

        // Destruir el objeto recolectado
        Destroy(gameObject);
        InventarioManager.instancia.AñadirADN(itemName, icon);
        Destroy(gameObject);
    }


}
