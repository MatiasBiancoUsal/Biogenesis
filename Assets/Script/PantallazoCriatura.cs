using System.Collections;
using UnityEngine;

public class PantallazoCriatura : MonoBehaviour
{
    public GameObject pantallazoUI; // arrastrás la Image del Canvas
    public float duracion = 3f;

    private bool mostrado = false;

    private void Start()
    {
        pantallazoUI.SetActive(false);
    }

    private void Update()
    {
        if (!mostrado && CriaturaCreada.Instance != null && CriaturaCreada.Instance.criaturaCreada)
        {
            StartCoroutine(MostrarPantallazo());
            mostrado = true;
        }
    }

    IEnumerator MostrarPantallazo()
    {
        pantallazoUI.SetActive(true);
        yield return new WaitForSeconds(duracion);
        pantallazoUI.SetActive(false);
    }
}
