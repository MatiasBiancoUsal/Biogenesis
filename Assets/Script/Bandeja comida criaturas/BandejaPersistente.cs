using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BandejaPersistente : MonoBehaviour
{
    private static BandejaPersistente instancia;

    [Header("Escenas donde la bandeja está permitida")]
    public string[] escenasPermitidas;

    [Header("Canvas Group de la bandeja")]
    public CanvasGroup canvasGroup; // arrastralo en el inspector

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool habilitada = false;

        foreach (string nombre in escenasPermitidas)
        {
            if (scene.name == nombre)
            {
                habilitada = true;
                break;
            }
        }

        MostrarBandeja(habilitada);
    }

    void MostrarBandeja(bool mostrar)
    {
        if (canvasGroup == null) return;

        if (mostrar)
        {
            canvasGroup.alpha = 1f;           // visible
            canvasGroup.interactable = true; // permite clicks
            canvasGroup.blocksRaycasts = true; // bloquea raycasts detrás
        }
        else
        {
            canvasGroup.alpha = 0f;           // invisible
            canvasGroup.interactable = false; // no clickeable
            canvasGroup.blocksRaycasts = false; // no bloquea
        }
    }
}
