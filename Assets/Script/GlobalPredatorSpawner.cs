using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPredatorSpawner : MonoBehaviour
{
    public GameObject predatorPrefab;
    public string[] sceneNames;
    public UIWarningMessage uiWarning; // Referencia al UI que muestra la alerta

    public static GlobalPredatorSpawner instance;
    private GameObject currentPredator;
    private string originalSceneName;
    public string randomScene;


    // Start is called before the first frame update
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            yield return new WaitForSeconds(120); // Cambiar a 300 para 5 min reales

            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("Tu criatura está siendo atacada en " + randomScene);

            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Tu criatura está siendo atacada en " + randomScene);
            }

            // Guardar escena actual
            originalSceneName = SceneManager.GetActiveScene().name;

            // Cargar escena de ataque (sustituyendo la actual)
           // AsyncOperation load = SceneManager.LoadSceneAsync(randomScene, LoadSceneMode.Single);
            //while (!load.isDone) yield return null;

            yield return null;

            //GameObject spawnPoint = GameObject.FindGameObjectWithTag("PredatorSpawn");
            //if (spawnPoint != null)
            //{
            //    currentPredator = Instantiate(predatorPrefab, spawnPoint.transform.position, Quaternion.identity);
            //}

            // Esperar a que el depredador muera
            while (currentPredator != null)
            {
                yield return null;
            }

            // Volver a la escena original
            //AsyncOperation back = SceneManager.LoadSceneAsync(originalSceneName, LoadSceneMode.Single);
            //while (!back.isDone) yield return null;
        }
    }

    public void PredatorDied()
    {
        currentPredator = null;
    }
}
