using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCriaturaExperimento : MonoBehaviour
{
    public InventarioManager InventarioManager; // Lo arrastrás desde el inspector

    private SpriteRenderer sr;

    public GameObject imgNombre;

    public GameObject barraHP;

    public GameObject barraComida;
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

            imgNombre.SetActive(true);
            barraHP.SetActive(true);
            barraComida.SetActive(true);
        }
    }
}
