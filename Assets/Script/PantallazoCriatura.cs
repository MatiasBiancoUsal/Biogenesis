using System.Collections;
using UnityEngine;

public class PantallazoCriatura : MonoBehaviour
{
    [Tooltip("Arrastra aqu� la imagen o panel de la UI que quieres mostrar.")]
    public GameObject pantallazoUI;

    [Tooltip("Tiempo en segundos que la imagen permanecer� en pantalla.")]
    public float duracion = 3f;

    // Variable est�tica para asegurar que solo se muestre una vez por sesi�n de juego.
    private static bool yaMostrado = false;

    // Al activarse el objeto, nos suscribimos para recibir noticias del manager.
    private void OnEnable()
    {
        InventarioManagerPrueba.OnCriaturaCreada += DispararPantallazo;
    }

    // Al desactivarse, nos damos de baja para evitar errores.
    private void OnDisable()
    {
        InventarioManagerPrueba.OnCriaturaCreada -= DispararPantallazo;
    }

    // Este m�todo es llamado por el evento 'OnCriaturaCreada' desde el manager.
    public void DispararPantallazo()
    {
        // Solo procedemos si no se ha mostrado antes y si la UI est� asignada.
        if (!yaMostrado && pantallazoUI != null)
        {
            StartCoroutine(MostrarPantallazoRutina());
        }
    }

    // La corutina que muestra y oculta la UI.
    private IEnumerator MostrarPantallazoRutina()
    {
        // Pre-activamos el objeto por si estaba desactivado
        pantallazoUI.SetActive(true);

        // Marcamos como mostrado para que no se repita
        yaMostrado = true;

        // Esperamos el tiempo definido
        yield return new WaitForSeconds(duracion);

        // Ocultamos la UI
        pantallazoUI.SetActive(false);
    }
}