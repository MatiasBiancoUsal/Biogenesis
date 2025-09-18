using UnityEngine;

public class Personaje : MonoBehaviour
{
    public int vida = 100;
    public int vidaMaxima = 100;

    private Animator animator;
    private bool estaMuerto = false;

    public enum TipoMutacion { Mutacion1, Mutacion2, Mutacion3 }
    public TipoMutacion mutacionActual;

    [Header("Sonidos")]
    public AudioClip sonidoDerrota;
    public AudioClip sonidoDaño;

    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("Falta AudioSource en el personaje.");
        }
    }

    public void TomarDaño(int cantidad)
    {
        if (estaMuerto) return;

        vida -= cantidad;

        // ▶️ Reproducir sonido de daño
        if (sonidoDaño != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDaño);
        }

        Debug.Log("Personaje recibió daño. Vida actual: " + vida);

        if (vida <= 0 && !estaMuerto)
        {
            vida = 0;
            Debug.Log("Personaje ha muerto");
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

        // ▶️ Reproducir sonido de muerte
        if (sonidoDerrota != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDerrota);
        }

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