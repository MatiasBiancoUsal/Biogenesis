using UnityEngine;
using System.Collections;
public class ControladorPantallazo : MonoBehaviour
{
    public GameObject objetoVisual;
    [Tooltip("Tiempo en segundos que el pantallazo permanecerá visible.")]
    public float duracionVisible = 3f;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("PANTALLAZO dice: '¡Estoy despierto! Me voy a suscribir a OnCriaturaCreada.'", this.gameObject);
        InventarioManagerPrueba.OnCriaturaCreada += IniciarSecuencia;
    }

    void OnDestroy()
    {
        // Es buena práctica desuscribirse al destruir el objeto
        InventarioManagerPrueba.OnCriaturaCreada -= IniciarSecuencia;
    }

    void IniciarSecuencia()
    {
        // Detenemos cualquier coroutine anterior para evitar que se solapen.
        StopAllCoroutines();
        StartCoroutine(SecuenciaDeVisualizacion());
    }

    // 3. Esta es la Coroutine que maneja la secuencia de tiempo.
    IEnumerator SecuenciaDeVisualizacion()
    {
        // Paso A: Muestra el objeto.
        if (objetoVisual != null)
        {
            objetoVisual.SetActive(true);
        }

        // Paso B: Espera por la cantidad de segundos que definimos.
        yield return new WaitForSeconds(duracionVisible);

        // Paso C: Esconde el objeto.
        if (objetoVisual != null)
        {
            objetoVisual.SetActive(false);
        }
    }
}