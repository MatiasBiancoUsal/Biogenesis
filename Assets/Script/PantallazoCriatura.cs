using System.Collections;
using UnityEngine;

public class PantallazoCriatura : MonoBehaviour
{
    public GameObject pantallazoUI; // La imagen dentro del Canvas
    public float duracion = 3f;     // Tiempo que dura el pantallazo
    private static bool yaMostrado = false;

    void Start()
    {
        if (pantallazoUI != null)
            pantallazoUI.SetActive(false); // Oculto al inicio
    }

    void Update()
    {
        // --- LA LÍNEA CORREGIDA ---
        // Ahora escuchamos al manager correcto, el que tiene la información persistente.
        if (!yaMostrado && InventarioManagerPrueba.instancia != null && InventarioManagerPrueba.instancia.criaturaCreada)
        {
            StartCoroutine(MostrarPantallazoRutina());
        }
    }

    private IEnumerator MostrarPantallazoRutina()
    {
        // Esta parte ya estaba perfecta.
        if (pantallazoUI != null) pantallazoUI.SetActive(true);
        yaMostrado = true;
        yield return new WaitForSeconds(duracion);
        if (pantallazoUI != null) pantallazoUI.SetActive(false);
    }
}
