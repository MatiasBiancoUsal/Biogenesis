using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlimañaDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    public float rangoDeteccion = 6f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip disparoClip;

    private bool disparando = false;

    // --- NUEVO ---
    // Referencia al script del personaje para saber su estado.
    private Personaje personaje;

    void Start()
    {
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

        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !disparando)
        {
            Debug.Log($"[Alimaña] Enemigo detectado: {enemigo.name}, comenzando ataque.");
            StartCoroutine(DispararRoutine(enemigo.transform));
        }
    }

    GameObject DetectarEnemigo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        AutoMover movimiento = GetComponent<AutoMover>();
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito"))
            {
                Debug.Log($"[Alimaña] Detectado enemigo en rango: {hit.name}");
                movimiento.quieto = true;
                return hit.gameObject;
            }
        }
        movimiento.quieto = false;
        return null;
    }

    IEnumerator DispararRoutine(Transform objetivo)
    {
        disparando = true;
        while (objetivo != null)
        {
            // --- NUEVO ---
            // Comprobamos la vida también dentro del bucle para detenerlo a mitad.
            if (personaje != null && personaje.vida <= 0)
            {
                break; // Sale del bucle de disparo si muere.
            }

            Debug.Log("[Alimaña] Disparando proyectil...");
            Disparar(objetivo);
            yield return new WaitForSeconds(intervaloDisparo);
        }
        disparando = false;
        Debug.Log("[Alimaña] Dejó de disparar (objetivo perdido).");
    }

    // ... el resto de tu código (Disparar, PlayShootSound, etc.) se mantiene igual ...

    void Disparar(Transform objetivo)
    {
        if (proyectilPrefab != null && puntoDisparo != null)
        {
            GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            proyectil.tag = "ProyectilEnemigo";

            var scriptProyectil = proyectil.GetComponent<ProyectilExplosivo>();
            scriptProyectil.direccion = (objetivo.position - puntoDisparo.position).normalized;

            Debug.Log($"[Alimaña] Proyectil creado hacia {objetivo.name}");

            MutacionAlimaña mutacion = GetComponent<MutacionAlimaña>();
            if (mutacion != null)
            {
                scriptProyectil.daño = Mathf.RoundToInt(scriptProyectil.daño * mutacion.ObtenerMultiplicadorDaño());
                Debug.Log($"[Alimaña] Proyectil modificado por mutación, daño final: {scriptProyectil.daño}");
            }

            PlayShootSound();
        }
        else
        {
            Debug.LogWarning("[Alimaña] No hay prefab o punto de disparo asignado.");
        }
    }

    void PlayShootSound()
    {
        if (audioSource != null && disparoClip != null)
        {
            audioSource.PlayOneShot(disparoClip);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}