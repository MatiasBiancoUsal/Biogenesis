using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPredatorSpawner : MonoBehaviour
{
    [Header("General")]
    public GameObject predatorPrefab;
    public string[] sceneNames;
    private UIWarningMessage uiWarning; // Ahora es privada y se encuentra en tiempo de ejecución.

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip alarmaClip;

    public static GlobalPredatorSpawner instance;
    private GameObject currentPredator;
    private string originalSceneName;
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
        StartCoroutine(PredatorLoop());
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

    public void SpawnearBicho()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PredatorSpawn");
        if (spawnPoint != null)
        {
            currentPredator = Instantiate(predatorPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    IEnumerator PredatorLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10); // 300 son 5 minutos

            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("Tu criatura está siendo atacada en " + randomScene);

            // Verifica si la alerta UI está disponible antes de intentar usarla.
            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Tu criatura está siendo atacada en " + randomScene);
            }
            else
            {
                Debug.LogError("UIWarningMessage no se encontró en la escena actual.");
            }

            if (audioSource != null && alarmaClip != null)
            {
                audioSource.PlayOneShot(alarmaClip);
            }

            originalSceneName = SceneManager.GetActiveScene().name;

            yield return null;

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