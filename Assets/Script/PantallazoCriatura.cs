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
        // Escuchar cuando se crea la criatura
        if (!yaMostrado && CriaturaCreada.Instance != null && CriaturaCreada.Instance.criaturaCreada)
        {
            StartCoroutine(MostrarPantallazoRutina());
        }
    }

    private IEnumerator MostrarPantallazoRutina()
    {
        pantallazoUI.SetActive(true);
        yaMostrado = true;
        yield return new WaitForSeconds(duracion);
        pantallazoUI.SetActive(false);
    }
}
