using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaturaDispara : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float velocidadProyectil = 5f;
    public float intervaloDisparo = 1.5f;
    public GameObject objetivo; // Depredador

    //private bool disparando = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ControlarDisparo());

    }

    void Disparar()
    {
        if (proyectilPrefab != null && puntoDisparo != null)
        {
            GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            proyectil.tag = "ProyectilEnemigo";

            // Apuntar al Depredador si existe
            if (objetivo != null)
            {
                Vector2 direccion = (objetivo.transform.position - puntoDisparo.position).normalized;
               // proyectil.GetComponent<proyectilcazador>().direccion = direccion;
            }
        }
    }

    IEnumerator ControlarDisparo()
    {
        while (true)
        {
            // Si el objetivo está en escena
            if (objetivo != null)
            {
                Disparar();
                yield return new WaitForSeconds(intervaloDisparo);
            }
            else
            {
                // Si el Depredador fue destruido, esperamos sin disparar
                yield return null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
