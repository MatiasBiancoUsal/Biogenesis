using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilExplosivo : MonoBehaviour
{
    public float velocidad = 7f;
    public int da�o = 1;
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
                depredador.RecibirDa�o();
            }

            Destroy(gameObject); // Se destruye el proyectil tras golpear
        }

        // Hongo par�sito
        var hongo = collision.GetComponent<ParasitoHongo>();
        if (hongo != null)
        {
            hongo.RecibirDa�o();
            Destroy(gameObject);
            return;
        }

        // En el futuro pod�s agregar m�s enemigos ac�
    }
}