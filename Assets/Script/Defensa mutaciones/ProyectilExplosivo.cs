using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MutacionAlima�a;

public class ProyectilExplosivo : MonoBehaviour
{
    public float velocidad = 7f;
    public int da�o = 1;
    public Vector2 direccion;
    public float tiempoVida = 4f;

    public Sprite spriteNormal;
    public Sprite spritePrimera;
    public Sprite spriteFinal;

    private SpriteRenderer sr;

    private Animator anim;
    private bool explotando = false;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Buscar a la mutaci�n
        MutacionAlima�a mutacion = FindFirstObjectByType<MutacionAlima�a>();
        if (mutacion != null)
        {
            //  AQU� definimos 'estado'
            EstadoMutacion estado = mutacion.ObtenerEstadoMutacion();

            switch (estado)
            {
                case EstadoMutacion.Normal:
                    if (spriteNormal != null) sr.sprite = spriteNormal;
                    break;

                case EstadoMutacion.Primera:
                    if (spritePrimera != null) sr.sprite = spritePrimera;
                    break;

                case EstadoMutacion.Final:
                    if (spriteFinal != null) sr.sprite = spriteFinal;
                    break;
            }

            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                switch (estado)
                {
                    case EstadoMutacion.Normal:
                        anim.SetTrigger("Normal");
                        break;
                    case EstadoMutacion.Primera:
                        anim.SetTrigger("Primera");
                        break;
                    case EstadoMutacion.Final:
                        anim.SetTrigger("Final");
                        break;
                }
            }
        }

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

       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!explotando)
        {
            Explotar();
        }
    }

    void Explotar()
    {
        explotando = true;

        if (anim != null)
        {
            anim.SetTrigger("Explotar");
        }

        // Si quer�s desactivar el collider para que no choque m�s:
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Opcional: detener el movimiento
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;
    }

    public void Destruir()
    {
        Destroy(gameObject);
    }
  
}