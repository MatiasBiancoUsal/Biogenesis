using UnityEngine;
using System.Collections;

public class ParasitoHongo : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject bolaPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    private Animator animator;
    private GameObject objetivo;

    [Header("Vida")]
    public int vida = 3;
    public Color colorDaño = Color.red;
    private Color colorOriginal;
    private SpriteRenderer spriteRenderer;
    public float tiempoDaño = 0.2f;

    [Header("Audio")]
    public AudioSource audioSource;   // Componente de audio
    public AudioClip spawnClip;       // Sonido al aparecer
    public AudioClip attackClip;      // Sonido al disparar
    public AudioClip deathClip;       // Sonido al morir

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        DetectarObjetivo();
        StartCoroutine(Comportamiento());

        // 🔊 Reproducir sonido al aparecer
        if (audioSource != null && spawnClip != null)
        {
            audioSource.PlayOneShot(spawnClip);
        }

        // Destruye el hongo a los 30 segundos
        Destroy(gameObject, 30f);
    }

    IEnumerator Comportamiento()
    {
        while (true)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(2f);

            animator.SetTrigger("Atacar");

            float tiempo = 5f;
            float t = 0f;
            while (t < tiempo)
            {
                LanzarBola();
                yield return new WaitForSeconds(intervaloDisparo);
                t += intervaloDisparo;
            }
        }
    }

    void LanzarBola()
    {
        GameObject bola = Instantiate(bolaPrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();

        Vector2 direccion = new Vector2(Random.Range(0.3f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.velocity = direccion * 1.2f;

        float escala = Random.Range(0.15f, 0.31f);
        bola.transform.localScale = new Vector3(escala, escala, 1f);

        // 🔊 Sonido al disparar
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }

    void DetectarObjetivo()
    {
        GameObject encontrado = GameObject.FindGameObjectWithTag("Criatura");
        if (encontrado != null)
        {
            objetivo = encontrado;
        }
    }

    public void RecibirDaño()
    {
        vida--;
        Debug.Log("☠️ Hongo recibió daño. Vida restante: " + vida);
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
        // 🔊 Sonido al morir
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }

        Destroy(gameObject, 0.3f); // un pequeño delay para que suene
    }
}