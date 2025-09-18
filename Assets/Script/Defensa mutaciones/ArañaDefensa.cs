using System.Collections;
using UnityEngine;

public class ArañaDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    public float rangoDeteccion = 6f;

    private bool disparando = false;

    [Header("Audio")]
    public AudioSource audioSource;   // Componente de audio
    public AudioClip disparoClip;     // Sonido del disparo de telaraña

    void Update()
    {
        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !disparando)
        {
            StartCoroutine(DispararRoutine(enemigo.transform));
        }
    }

    GameObject DetectarEnemigo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        DerivadoAutoMover movimiento = GetComponent<DerivadoAutoMover>();
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito")) // ✅ Ahora detecta los dos
            {
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
            Disparar(objetivo);
            yield return new WaitForSeconds(intervaloDisparo);
        }
        disparando = false;
    }

    void Disparar(Transform objetivo)
    {
        if (proyectilPrefab != null && puntoDisparo != null)
        {
            GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            proyectil.tag = "ProyectilEnemigo";

            Vector2 direccion = (objetivo.position - puntoDisparo.position).normalized;
            proyectil.GetComponent<ProyectilTelaraña>().direccion = direccion;

            // 🔊 Reproducir sonido de disparo
            PlayShootSound();
        }
    }

    // --------------------
    // Audio
    // --------------------
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