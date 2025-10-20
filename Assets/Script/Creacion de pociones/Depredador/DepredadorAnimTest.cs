using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class DepredadorAnimTest : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public GameObject cuchilloPrefab;
    public Transform puntoDisparo;
    public float velocidadCuchillo = 7f;
    public GameObject objetivo;

    public int vida = 3;
    public Color colorDaño = Color.red;
    private Color colorOriginal;
    public float tiempoDaño = 0.2f;

    [Header("Audio")]
    public AudioSource audioSource;   // Componente AudioSource
    public AudioClip spawnClip;       // Sonido al aparecer
    public AudioClip ataqueClip;      // Sonido al disparar cuchillo
    public AudioClip muerteClip;      // (Opcional) Sonido al morir

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;

        DetectarObjetivo();

        // 🔊 Sonido al aparecer
        if (audioSource != null && spawnClip != null)
        {
            audioSource.PlayOneShot(spawnClip);
        }

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

            // 🔊 Sonido al disparar cuchillo
            if (audioSource != null && ataqueClip != null)
            {
                audioSource.PlayOneShot(ataqueClip);
            }
        }
    }

    public void RecibirDaño()
    {
        vida--;
        StartCoroutine(FlashRojo());

        if (vida <= 0)
        {
            Morir();
        }
        CustomEvent derrota = new CustomEvent("enemigo_derrotado")
                {
                    { "derrota_enemigo", gameObject.tag}
                };
        AnalyticsService.Instance.RecordEvent(derrota);
        print("evento " + "enemigo_derrotado " + gameObject.tag);
        AnalyticsService.Instance.Flush();
        //
    }

    IEnumerator FlashRojo()
    {
        spriteRenderer.color = colorDaño;
        yield return new WaitForSeconds(tiempoDaño);
        spriteRenderer.color = colorOriginal;
    }

    void Morir()
    {
        // 🔊 Sonido de muerte (opcional)
        if (audioSource != null && muerteClip != null)
        {
            audioSource.PlayOneShot(muerteClip);
        }

        // ⚡ Podrías poner animación de muerte antes de destruirlo
        Destroy(gameObject, 0.5f);
    }

    void DetectarObjetivo()
    {
        string[] posiblesTags = { "Criatura", "Criatura1", "Criatura2", "Criatura3" };

        foreach (string tag in posiblesTags)
        {
            GameObject encontrado = GameObject.FindGameObjectWithTag(tag);
            if (encontrado != null)
            {
                objetivo = encontrado;
                break;
            }
        }

        if (objetivo == null)
        {
            Debug.LogWarning("❗ Depredador no encontró objetivo en esta escena.");
        }
    }
}