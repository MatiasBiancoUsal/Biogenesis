using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajonManager : MonoBehaviour
{
    public GameObject cajonPrefab;        // sprite normal
    public GameObject cajonRotoPrefab;    // sprite roto
    public Transform spawnPosition;
    public float interval = 5f;

    private GameObject currentCajon;
    private float timer = 0f;

    [Header("Audio")]
    public AudioClip spawnSound;           // sonido cuando se rompe
    [Range(0f, 2f)] public float spawnVolume = 1f;

    private AudioSource audioSource;

    void Start()
    {
        timer = 0f;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.loop = false;
        audioSource.volume = spawnVolume;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;

            // Si no hay caj�n, instanciar el roto
            if (currentCajon == null)
            {
                currentCajon = Instantiate(cajonRotoPrefab, spawnPosition.position, Quaternion.identity);

                // Sonido en loop
                if (spawnSound != null)
                {
                    audioSource.clip = spawnSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    // M�todo para "arreglar" el caj�n
    public void ArreglarCajon()
    {
        if (currentCajon != null)
        {
            Vector3 pos = currentCajon.transform.position;
            Destroy(currentCajon);

            // Instanciar el caj�n normal en la misma posici�n
            currentCajon = Instantiate(cajonPrefab, pos, Quaternion.identity);
        }

        // Detener el sonido
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
