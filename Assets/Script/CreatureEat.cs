using UnityEngine;

public class CreatureEat : MonoBehaviour
{
    [Header("Opcionales")]
    public HungerBar hungerBar; // Opcional: para subir barra de hambre si querés

    [Header("Audio")]
    public AudioClip sonidoComer;         // Clip de sonido de comer (asignalo desde el Inspector)
    public AudioSource audioSourceComida; // Fuente de audio que reproducirá el sonido

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(">>> Trigger detectado con: " + collision.name);

        // Detecta si el objeto tiene el tag "Food"
        if (collision.CompareTag("Food"))
        {
            Debug.Log(">>> Tag 'Food' confirmado");

            // Reproducir sonido de comer
            if (sonidoComer != null && audioSourceComida != null)
            {
                audioSourceComida.PlayOneShot(sonidoComer);
                Debug.Log(">>> Sonido de comer reproducido");
            }
            else
            {
                Debug.LogWarning(">>> FALTA asignar el AudioClip o el AudioSource en el Inspector");
            }

            // OPCIONAL: Subir barra de hambre
            if (hungerBar != null)
            {
                hungerBar.Feed(0.5f); // Cambiá el valor según lo que necesites
            }

            // OPCIONAL: Destruir la comida después de comer
            Destroy(collision.gameObject);
        }
    }
}
