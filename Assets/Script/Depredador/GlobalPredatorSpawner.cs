using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPredatorSpawner : MonoBehaviour
{
    [Header("General")]
    public GameObject predatorPrefab;
    public string[] sceneNames;
    public UIWarningMessage uiWarning;

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

    void Start() { }

    void Update() { }

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
            yield return new WaitForSeconds(300); // Cambiar a 300 para 5 min reales

            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("Tu criatura está siendo atacada en " + randomScene);

            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Tu criatura está siendo atacada en " + randomScene);
            }

            // 🔊 Reproducir sonido de alarma
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