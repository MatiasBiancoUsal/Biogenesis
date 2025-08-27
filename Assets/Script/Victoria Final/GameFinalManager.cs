using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Cantidad de criaturas a mutar")]
    public int totalCriaturas = 4; // ajustalo según cuántas criaturas tengas
    private int criaturasMutadasFinal = 0;

    private bool experimentoCreado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NotificarCriaturaMutadaFinal()
    {
        criaturasMutadasFinal++;
        Debug.Log("Criatura llegó a su mutación final ({criaturasMutadasFinal}/{totalCriaturas})");
        RevisarVictoria();
    }

    public void NotificarExperimentoCreado()
    {
        experimentoCreado = true;
        Debug.Log("Experimento creado");
        RevisarVictoria();
    }

    private void RevisarVictoria()
    {
        if (criaturasMutadasFinal >= totalCriaturas && experimentoCreado)
        {
            Debug.Log("¡Victoria cumplida! Cargando escena final...");
            SceneManager.LoadScene("EscenaFinalDelJuego"); // usa el nombre exacto de tu escena
        }
    }
}
