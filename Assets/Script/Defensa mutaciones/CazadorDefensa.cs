using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public float intervaloAtaque = 1.5f;
    public float rangoDeteccion = 2f; // Rango cuerpo a cuerpo
    public int daño = 10;

    private bool atacando = false;
    private Animator anim;

    [Header("Audio")]
    public AudioSource audioSource;    // El componente de audio
    public AudioClip attackClip;       // Sonido del ataque/poder

    public enum EstadoMutacion { Normal, Primera, Final }
    public EstadoMutacion estado = EstadoMutacion.Normal;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Iniciar ciclo de animaciones
        StartCoroutine(CicloAnimaciones());
    }

    void Update()
    {
        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !atacando)
        {
            Debug.Log($"[Cazador] Enemigo detectado: {enemigo.name}, comenzando ataque.");
            StartCoroutine(AtacarRoutine(enemigo.transform));
        }
    }

    // --------------------
    // Detección de enemigos
    // --------------------
    GameObject DetectarEnemigo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito"))
            {
                Debug.Log($"[Cazador] Detectado enemigo en rango: {hit.name}");
                return hit.gameObject;
            }
        }
        return null;
    }

    // --------------------
    // Rutina de ataque
    // --------------------
    IEnumerator AtacarRoutine(Transform objetivo)
    {
        atacando = true;
        while (objetivo != null)
        {
            Atacar(objetivo);
            yield return new WaitForSeconds(intervaloAtaque);
        }
        atacando = false;
        Debug.Log("[Cazador] Dejó de atacar (objetivo perdido).");
    }

    // --------------------
    // Método de ataque
    // --------------------
    void Atacar(Transform objetivo)
    {
        // Animación según mutación
        switch (estado)
        {
            case EstadoMutacion.Normal: anim.SetTrigger("Atacar1"); break;
            case EstadoMutacion.Primera: anim.SetTrigger("Atacar2"); break;
            case EstadoMutacion.Final: anim.SetTrigger("Atacar3"); break;
        }

        // Reproducir sonido de ataque
        PlayAttackSound();

        // Aplicar daño al objetivo
        DepredadorAnimTest depredador = objetivo.GetComponent<DepredadorAnimTest>();
        ParasitoHongo hongo = objetivo.GetComponent<ParasitoHongo>();

        if (depredador != null)
        {
            depredador.RecibirDaño();
            Debug.Log($"[Cazador] Infligió 1 de daño a depredador {objetivo.name}");
        }
        else if (hongo != null)
        {
            hongo.RecibirDaño();
            Debug.Log($"[Cazador] Infligió 1 de daño a parásito {objetivo.name}");
        }
        else
        {
            Debug.LogWarning($"[Cazador] No se encontró script de vida en el objetivo: {objetivo.name}");
        }
    }

    // --------------------
    // Ciclo de animaciones básicas (Idle/Disparar)
    // --------------------
    IEnumerator CicloAnimaciones()
    {
        while (true)
        {
            anim.SetTrigger("idle");
            yield return new WaitForSeconds(2f);
        }
    }

    // --------------------
    // Audio
    // --------------------
    void PlayAttackSound()
    {
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip); // sonido puntual
        }
    }

    // --------------------
    // Visualizar rango de detección en el editor
    // --------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
