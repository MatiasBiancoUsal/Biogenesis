using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("Vida")]
    public int vida = 100;
    public int vidaMaxima = 100;

    [Header("Audio")]
    public AudioClip sonidoHerida;
    public AudioClip sonidoMuerte;

    private AudioSource audioSource;
    private Animator animator;
    private bool estaMuerto = false;

    public enum TipoMutacion { Mutacion1, Mutacion2, Mutacion3 }
    public TipoMutacion mutacionActual;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("No se encontró un AudioSource en el objeto. Agregalo para que suenen los efectos.");
        }
    }

    public void TomarDaño(int cantidad)
    {
        if (estaMuerto) return;

        vida -= cantidad;

        // Reproducir sonido de herida
        if (sonidoHerida != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoHerida);
        }

        Debug.Log("Personaje recibió daño. Vida actual: " + vida);

        if (vida <= 0)
        {
            vida = 0;
            Morir();
        }
    }

    public void RestaurarVida(int cantidad)
    {
        if (vida < vidaMaxima)
        {
            vida += cantidad;
            if (vida > vidaMaxima)
            {
                vida = vidaMaxima;
            }

            Debug.Log("Personaje se curó. Vida actual: " + vida);
        }
    }

    private void Morir()
    {
        estaMuerto = true;

        Debug.Log("Personaje ha muerto");

        // Reproducir sonido de muerte
        if (sonidoMuerte != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoMuerte);
        }

        // Disparar animación según mutación
        switch (mutacionActual)
        {
            case TipoMutacion.Mutacion1:
                animator.SetTrigger("muerte1");
                break;
            case TipoMutacion.Mutacion2:
                animator.SetTrigger("muerte2");
                break;
            case TipoMutacion.Mutacion3:
                animator.SetTrigger("muerte3");
                break;
        }
    }

    public void DestruirObjeto()
    {
        Debug.LogWarning("!!! FUNCIÓN DESTRUIR OBJETO LLAMADA !!! La animación de muerte terminó o fue activada por error.");
        Destroy(gameObject);
    }
}