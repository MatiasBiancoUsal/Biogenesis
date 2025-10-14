using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalParasiteSpawner : MonoBehaviour
{
    [Header("Parásitos")]
    public GameObject parasitePrefab;
    public string[] sceneNames;
    private UIWarningMessage uiWarning; // Ahora se encuentra en tiempo de ejecución.

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip alarmaClip;

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

    void OnEnable()
    {
        // Se suscribe a un evento que se activa cuando una escena se carga.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Se desuscribe del evento para evitar problemas.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Este método se llama cada vez que se carga una escena.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Busca y asigna el componente UIWarningMessage de la nueva escena.
        uiWarning = FindObjectOfType<UIWarningMessage>();
    }

    IEnumerator ParasiteLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(300); // 

            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("🌿 Ataque parasitario en " + randomScene);

            // Verifica si la alerta UI está disponible antes de intentar usarla.
            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Un hongo parasitario está atacando en " + randomScene);
            }
            else
            {
                Debug.LogError("UIWarningMessage no se encontró en la escena actual.");
            }

            // Reproducir sonido de alarma
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
            {
                Destroy(currentParasite);
            }
        }
    }

    public void SpawnearParásito()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("ParasiteSpawn");
        if (spawnPoint != null)
        {
            currentParasite = Instantiate(parasitePrefab, spawnPoint.transform.position, Quaternion.identity);

            //evento depredador
            CustomEvent enemigo = new CustomEvent("enemigo_aparecio")
        {
        { "tipo_enemigo", "parasito" }

     };
            AnalyticsService.Instance.RecordEvent(enemigo);
            AnalyticsService.Instance.Flush();
            //
        }
    }
}