using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("Vida")]
    public int vida = 100;
    public int vidaMaxima = 100;

    private Animator animator;
    private bool estaIncapacitado = false;

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
            Debug.LogWarning("⚠️ Falta AudioSource en el personaje.");
        }

        // --- NUEVO ---
        // Al empezar, le decimos al Animator qué mutación tenemos.
        // El (int) convierte el enum (Mutacion1, Mutacion2) a un número (0, 1).
        animator.SetInteger("tipoMutacion", (int)mutacionActual);
    }

    void Update()
    {
        animator.SetBool("estaIncapacitado", vida <= 0);
    }

    // --- Opcional pero recomendado ---
    // Si la mutación puede cambiar en medio del juego, llama a esta función
    public void CambiarMutacion(TipoMutacion nuevaMutacion)
    {
        mutacionActual = nuevaMutacion;
        animator.SetInteger("tipoMutacion", (int)mutacionActual);
        Debug.Log("🧬 Mutación cambiada a: " + nuevaMutacion);
    }

    public void TomarDaño(int cantidad)
    {
        // ... (el resto de la función TomarDaño se mantiene igual)
        if (estaIncapacitado) return;

        vida -= cantidad;
        Debug.Log("💥 Personaje recibió daño. Vida actual: " + vida);

        if (sonidoDaño != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDaño);
        }

        if (vida <= 0)
        {
            vida = 0;
            if (!estaIncapacitado)
            {
                estaIncapacitado = true;
                Debug.Log("☠️ Personaje ha quedado incapacitado");

                if (sonidoDerrota != null && audioSource != null)
                {
                    audioSource.PlayOneShot(sonidoDerrota);
                }
            }
        }
    }

    public void RestaurarVida(int cantidad)
    {
        // ... (la función RestaurarVida se mantiene igual)
        if (vida <= 0 && cantidad > 0)
        {
            estaIncapacitado = false;
            Debug.Log("💚 ¡Personaje recuperado!");
        }

        vida += cantidad;
        if (vida > vidaMaxima)
        {
            vida = vidaMaxima;
        }
        Debug.Log("Vida actual: " + vida);

        //evento criatura curada

        //
    }
}
