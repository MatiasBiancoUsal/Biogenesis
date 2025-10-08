using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class criaturadefensa : MonoBehaviour
{
    [Header("Ataque")]
    public float intervaloAtaque = 1.5f;
    public float rangoDeteccion = 2f; // Rango cuerpo a cuerpo
    public int daño = 10;

    [Header("Audio")]
    public AudioSource audioSource;   // Componente de audio
    public AudioClip ataqueClip;      // Sonido del ataque cuerpo a cuerpo

    private bool atacando = false;
    private Animator anim;

    // --- NUEVO ---
    // Referencia al script del personaje para saber su estado.
    private Personaje personaje;

    void Start()
    {
        anim = GetComponent<Animator>();

        // --- NUEVO ---
        // Obtenemos el componente Personaje al iniciar.
        personaje = GetComponent<Personaje>();
    }

    void Update()
    {
        // --- NUEVO Y MUY IMPORTANTE ---
        // Si el personaje está incapacitado (vida <= 0), detenemos toda la lógica de ataque.
        if (personaje != null && personaje.vida <= 0)
        {
            return; // No se ejecuta nada más del Update.
        }

        GameObject criatura = DetectarCriatura();
        if (criatura != null && !atacando)
        {
            StartCoroutine(AtacarRoutine(criatura.transform));
        }
    }

    GameObject DetectarCriatura()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador"))
            {
                return hit.gameObject;
            }
        }
        return null;
    }

    IEnumerator AtacarRoutine(Transform objetivo)
    {
        atacando = true;
        while (objetivo != null)
        {
            // --- NUEVO ---
            // Comprobamos la vida también dentro del bucle para detenerlo a mitad.
            if (personaje != null && personaje.vida <= 0)
            {
                break; // Sale del bucle de ataque si muere.
            }

            Atacar(objetivo);
            yield return new WaitForSeconds(intervaloAtaque);
        }
        atacando = false;
    }

    void Atacar(Transform objetivo)
    {
        var criatura = objetivo.GetComponent<DepredadorAnimTest>();
        if (criatura != null)
        {
            criatura.RecibirDaño();
            Debug.Log($"[criatura] Infligió {daño} de daño a {objetivo.name}");

            if (anim != null)
                anim.SetTrigger("ataque1");

            PlayAttackSound();
        }
    }

    void PlayAttackSound()
    {
        if (audioSource != null && ataqueClip != null)
        {
            audioSource.PlayOneShot(ataqueClip);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}