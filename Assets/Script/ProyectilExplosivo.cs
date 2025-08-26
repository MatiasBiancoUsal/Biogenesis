using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilExplosivo : MonoBehaviour
{
    public float velocidad = 7f;
    public int daño = 1;
    public Vector2 direccion;
    public float tiempoVida = 4f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Depredador
        if (collision.CompareTag("depredador"))
        {
            DepredadorAnimTest depredador = collision.GetComponent<DepredadorAnimTest>();
            if (depredador != null)
            {
                depredador.RecibirDaño();
            }

            Destroy(gameObject); // Se destruye el proyectil tras golpear
        }

        // Hongo parásito
        var hongo = collision.GetComponent<ParasitoHongo>();
        if (hongo != null)
        {
            hongo.RecibirDaño();
            Destroy(gameObject);
            return;
        }

        // En el futuro podés agregar más enemigos acá
    }
}