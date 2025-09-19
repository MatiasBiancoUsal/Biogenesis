using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BandejaPersistente : MonoBehaviour
{
    private static BandejaPersistente instancia;

    [Header("Escenas donde la bandeja está permitida")]
    public string[] escenasPermitidas;

    //Nuevo Sofi
    [Header("Escena donde la bandeja va a la derecha")]
    public string escenaDerecha;
    //

    [Header("Canvas Group de la bandeja")]
    public CanvasGroup canvasGroup; // arrastralo en el inspector

    //Nuevo Sofi
    [Header("Panel de la bandeja")]
    public RectTransform bandejaPanel;

    private Vector2 posIzquierda = new Vector2(0f, 0.5f); // Ancla en el medio izquierdo
    private Vector2 posDerecha = new Vector2(1f, 0.5f); // Ancla en el medio derecho

    private Vector2 pivotIzquierda = new Vector2(0f, 0.5f); // Pivot a la izquierda
    private Vector2 pivotDerecha = new Vector2(1f, 0.5f); // Pivot a la derecha
    //

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
        bool enEscenaDerecha = (scene.name == escenaDerecha); //Nuevo Sofi

        foreach (string nombre in escenasPermitidas)
        {
            if (scene.name == nombre)
            {
                habilitada = true;
                break;
            }
        }

        MostrarBandeja(habilitada);
        CambiarPosicion(enEscenaDerecha); //Nuevo Sofi

        //Nuevo sofi, para que no aparezca abierta al cambiar de escena
        if (bandejaPanel != null)
        {
            bandejaPanel.gameObject.SetActive(false);
        }
    }

    //Cambio script sofi
    void MostrarBandeja(bool mostrar)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = mostrar ? 1f : 0f;
        canvasGroup.interactable = mostrar;
        canvasGroup.blocksRaycasts = mostrar;
    }

    void CambiarPosicion(bool enDerecha)
    {
        if (bandejaPanel == null) return;

        if (enDerecha)
        {
            // Coloca la bandeja a la derecha
            // Asume que los anclajes están en el lado derecho del canvas
            bandejaPanel.anchoredPosition = new Vector2(200, 0); // Ajusta este valor
        }
        else
        {
            // Coloca la bandeja a la izquierda
            // Asume que los anclajes están en el lado izquierdo del canvas
            bandejaPanel.anchoredPosition = new Vector2(-200, 0); // Ajusta este valor
        }
    }
    //
}
