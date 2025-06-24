using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscenaConRetraso : MonoBehaviour
{
    public float tiempoEspera = 5f; // Tiempo en segundos
    public string escenaDestino = "Escena Mapa"; // Nombre exacto de la escena siguiente

    void Start()
    {
        StartCoroutine(CargarConDelay());
    }

    IEnumerator CargarConDelay()
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene(escenaDestino);
    }
}

