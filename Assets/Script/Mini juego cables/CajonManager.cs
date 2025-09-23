using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajonManager : MonoBehaviour
{
    public GameObject cajonRotoPrefab;
    public Transform spawnPosition;
    public float interval = 5f; // cada cuánto aparece el cajón

    private GameObject currentCajon;
    private float timer = 0f;

    [Header("Audio")]
    public AudioClip spawnSound;   // sonido cuando aparece el cajón
    [Range(0f, 2f)] public float spawnVolume = 1f; // volumen ajustable (0 = mute, 1 = normal, 2 = boost)

    private AudioSource audioSource;

    void Start()
    {
        timer = 0f;

        // Creo y configuro un AudioSource para reproducir sonidos globales
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 0 = sonido global (2D)
        audioSource.volume = 1f;       // volumen base
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;

            // Reiniciar estado del minijuego
            PlayerPrefs.SetInt("MiniJuegoResuelto", 0);

            // Instanciar cajón si no existe
            if (currentCajon == null)
            {
                currentCajon = Instantiate(cajonRotoPrefab, spawnPosition.position, Quaternion.identity);

                // Reproducir sonido global al aparecer con volumen ajustable
                if (spawnSound != null)
                {
                    audioSource.PlayOneShot(spawnSound, spawnVolume);
                }
            }
        }

        // Si ya resolviste el minijuego, destruir el cajón si está en escena
        if (PlayerPrefs.GetInt("MiniJuegoResuelto", 0) == 1 && currentCajon != null)
        {
            Destroy(currentCajon);
        }
    }
}
