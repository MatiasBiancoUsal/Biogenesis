using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolectarADN : MonoBehaviour
{
    void OnMouseDown()
    {

        // Destruir el objeto recolectado
        Destroy(gameObject);
    }
}
