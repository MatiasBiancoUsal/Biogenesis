using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorCriatura : MonoBehaviour
{
    public InventarioManagerPrueba InventarioManagerPrueba; // Lo arrastrás desde el inspector

    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false; // Oculta el sprite al principio
    }

    void Update()
    {
        if (CriaturaCreada.Instance != null && CriaturaCreada.Instance.criaturaCreada)
        {
            sr.enabled = true; // Muestra el sprite cuando la criatura fue creada
        }
    }
}
