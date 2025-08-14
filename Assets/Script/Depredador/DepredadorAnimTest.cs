using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepredadorAnimTest : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public GameObject cuchilloPrefab;        // Prefab del cuchillo
    public Transform puntoDisparo;           // Desde dónde dispara
    public float velocidadCuchillo = 7f;     // Velocidad del cuchillo
    public GameObject objetivo;              // El objetivo (criatura)

    public int vida = 3;
    public Color colorDaño = Color.red;
    private Color colorOriginal;
    public float tiempoDaño = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;

        // Detectar la criatura en la escena activa
        DetectarObjetivo();

        StartCoroutine(CicloAnimaciones());
    }

    IEnumerator CicloAnimaciones()
    {
        while (true)
        {
            // IDLE
            animator.ResetTrigger("Disparar");
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(2f);

            // DISPARAR
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Disparar");

            float tiempoDisparo = 5f;
            float intervalo = 1.5f;
            float tiempoPasado = 0f;

            while (tiempoPasado < tiempoDisparo)
            {
                DispararCuchillo();
                yield return new WaitForSeconds(intervalo);
                tiempoPasado += intervalo;
            }
        }
    }

    void DispararCuchillo()
    {
        if (cuchilloPrefab != null && puntoDisparo != null)
        {
            GameObject cuchillo = Instantiate(cuchilloPrefab, puntoDisparo.position, Quaternion.identity);
            Rigidbody2D rb = cuchillo.GetComponent<Rigidbody2D>();

            if (objetivo != null)
            {
                Vector2 direccion = (objetivo.transform.position - puntoDisparo.position).normalized;
                rb.velocity = direccion * velocidadCuchillo;
            }
            else
            {
                rb.velocity = Vector2.right * velocidadCuchillo;
            }

            Destroy(cuchillo, 5f);
        }
    }

    // 💥 Recibir daño al ser golpeado por proyectil enemigo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ProyectilEnemigo"))
        {
            Destroy(collision.gameObject); // destruye el proyectil enemigo
            RecibirDaño();
        }
    }

    void RecibirDaño()
    {
        vida--;
        StartCoroutine(FlashRojo());

        if (vida <= 0)
        {
            Morir();
        }
    }

    IEnumerator FlashRojo()
    {
        spriteRenderer.color = colorDaño;
        yield return new WaitForSeconds(tiempoDaño);
        spriteRenderer.color = colorOriginal;
    }

    void Morir()
    {
        // Acá podés poner animación de muerte o partículas
        Destroy(gameObject);
    }

    void DetectarObjetivo()
    {
        // Busca un GameObject con uno de los posibles tags
        string[] posiblesTags = { "Criatura", "Criatura1", "Criatura2", "Criatura3" };

        foreach (string tag in posiblesTags)
        {
            GameObject encontrado = GameObject.FindGameObjectWithTag(tag);
            if (encontrado != null)
            {
                objetivo = encontrado;
                break; // Salir cuando encuentra uno
            }
        }

        if (objetivo == null)
        {
            Debug.LogWarning("❗ Depredador no encontró objetivo en esta escena.");
        }
    }
}
