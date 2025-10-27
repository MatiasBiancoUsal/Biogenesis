using System.Collections;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPredatorSpawner : MonoBehaviour
{
    [Header("General")]
    public GameObject predatorPrefab;
    public string[] sceneNames;
    private UIWarningMessage uiWarning;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip alarmaClip;

    public static GlobalPredatorSpawner instance;
    private GameObject currentPredator;
    private string randomScene;   // Escena que va a ser atacada

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(PredatorLoop());
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Siempre reasigna el UIWarning cuando cambias de escena
        uiWarning = FindObjectOfType<UIWarningMessage>();

        // 👇 Si entrás a la escena que fue marcada como atacada, spawnea al depredador
        if (scene.name == randomScene && currentPredator == null)
        {
            SpawnearBicho();
        }
    }

    public void SpawnearBicho()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PredatorSpawn");

        if (spawnPoint != null)
        {
            currentPredator = Instantiate(predatorPrefab, spawnPoint.transform.position, Quaternion.identity);
            Debug.Log("⚡ Depredador spawneado en " + spawnPoint.transform.position);
        }
        else
        {
            Debug.LogWarning("❗ No se encontró un objeto con tag 'PredatorSpawn' en la escena.");
        }


        //evento depredador
        CustomEvent enemigo = new CustomEvent("enemigo_aparecio")
        {
        { "tipo_enemigo", "depredador" }

     };
        AnalyticsService.Instance.RecordEvent(enemigo);
        AnalyticsService.Instance.Flush();
        //
    }

    IEnumerator PredatorLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(100f); // 180 es cada 3 minutos

            // Escena random de la lista
            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("⚠️ Tu criatura está siendo atacada en " + randomScene);

            // Mostrar alerta UI
            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Tu criatura está siendo atacada en " + randomScene);
            }
            else
            {
                Debug.LogWarning("UIWarningMessage no se encontró en la escena actual.");
            }

            // Reproducir sonido de alarma
            if (audioSource != null && alarmaClip != null)
            {
                audioSource.PlayOneShot(alarmaClip);
            }

            // Esperar hasta que ese depredador muera antes de volver a lanzar otro ataque
            while (currentPredator != null)
            {
                yield return null;
            }
        }
    }

    public void PredatorDied()
    {
        currentPredator = null;
    }
}
