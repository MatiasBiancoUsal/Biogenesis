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
    public AudioSource audioSource;
    public AudioClip disparoClip;

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
        SpriteRenderer img = GetComponent<SpriteRenderer>();

        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito"))
            {
                movimiento.quieto = true;

                // Voltea el sprite para mirar al enemigo.
                if (hit.transform.position.x < transform.position.x)
                {
                    img.flipX = true;
                }
                else
                {
                    img.flipX = false;
                }

                return hit.gameObject;
            }
        }

        // Si no se encuentra un enemigo, la criatura se mueve.
        movimiento.quieto = false;

        // Se asegura de que el sprite regrese a su orientación por defecto.
        img.flipX = false;

        return null;
    }

    IEnumerator DispararRoutine(Transform objetivo)
    {
        disparando = true;
        while (objetivo != null)
        {
            // Vuelve a verificar la dirección del objetivo antes de cada disparo, por si se mueve.
            if (objetivo.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

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

            PlayShootSound();
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