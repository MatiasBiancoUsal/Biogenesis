using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalParasiteSpawner : MonoBehaviour
{
    [Header("Parásitos")]
    public GameObject parasitePrefab;
    public string[] sceneNames;
    public UIWarningMessage uiWarning;

    [Header("Audio")] // 🔊 NUEVO
    public AudioSource audioSource;   // 🔊 Componente de audio
    public AudioClip alarmaClip;      // 🔊 Sonido de alarma

    public static GlobalParasiteSpawner instance;
    private GameObject currentParasite;
    public string randomScene;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(ParasiteLoop());
    }

    IEnumerator ParasiteLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(180); // 180 para 3 minutos

            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("🌿 Ataque parasitario en " + randomScene);

            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Un hongo parasitario está atacando en " + randomScene);
            }

            // 🔊 Reproducir sonido de alarma
            if (audioSource != null && alarmaClip != null)
            {
                audioSource.PlayOneShot(alarmaClip);
            }

            yield return null;

            float tiempoPresencia = 30f;
            float t = 0;
            while (currentParasite != null && t < tiempoPresencia)
            {
                t += Time.deltaTime;
                yield return null;
            }

            if (currentParasite != null)
                Destroy(currentParasite);
        }
    }

    public void SpawnearParásito()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("ParasiteSpawn");
        if (spawnPoint != null)
        {
            currentParasite = Instantiate(parasitePrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}