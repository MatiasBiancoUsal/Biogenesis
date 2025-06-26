using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalParasiteSpawner : MonoBehaviour
{
    public GameObject parasitePrefab;
    public string[] sceneNames;
    public UIWarningMessage uiWarning;

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
            yield return new WaitForSeconds(180); // 3 minutos

            randomScene = sceneNames[Random.Range(0, sceneNames.Length)];
            Debug.Log("🌿 Ataque parasitario en " + randomScene);

            if (uiWarning != null)
            {
                uiWarning.ShowWarning("Un hongo parasitario está atacando en " + randomScene);
            }

            // Guardar escena actual si querés hacer teleport (opcional)
            // originalScene = SceneManager.GetActiveScene().name;

            yield return null; // pequeño delay

            // Espera hasta que desaparezca
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
