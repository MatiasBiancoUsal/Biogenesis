using UnityEngine;

public class CreatureEat : MonoBehaviour
{
    public HungerBar hungerBar;         // Referencia a la barra de hambre
    public CriaturaComidaEsp comidaEsp; // Script con la comida aceptada

    [Header("Audio")]
    public AudioClip sonidoComer;           // Clip de sonido que se va a reproducir
    public AudioSource audioSourceComida;   // AudioSource asignado desde el Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(">>> Trigger detectado con: " + collision.name); // Verifica que se ejecute

        if (collision.CompareTag("Food"))
        {
            Debug.Log(">>> Tag 'Food' confirmado");

            // Verifica que el nombre de la comida contenga el nombre base de la comida aceptada
            if (collision.gameObject.name.Contains(comidaEsp.comidaAceptada.name))
            {
                // Alimentar al personaje
                hungerBar.Feed(0.2f);

                // Reproducir sonido de comer si está asignado
                if (sonidoComer != null && audioSourceComida != null)
                {
                    Debug.Log(">>> Reproduciendo sonido de comida");
                    audioSourceComida.PlayOneShot(sonidoComer);
                }

                Debug.Log($"{gameObject.name} comió {collision.name}!");

                // Destruir comida
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log($"{gameObject.name} rechazó {collision.name}");
            }
        }
    }
}