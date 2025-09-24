using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADNmutanteradioactivo : MonoBehaviour
{
    public GameObject prefabADN;
    public float intervalo = 5f;

    [Header("L�mites de Generaci�n")]
    public int maximoADNTotal = 9; // El m�ximo que el jugador puede tener en su inventario.
    public int minimoEnEscena = 4; // Intentar� que siempre haya este n�mero disponible.

    private float tiempoSiguiente;

    void Start()
    {
        tiempoSiguiente = intervalo;
    }

    void Update()
    {
        if (InventarioManagerPrueba.instancia == null) return; // Esperar a que el inventario est� listo

        // 1. Le preguntamos al inventario el total de ADN que ya tiene el jugador.
        int adnDelJugador = InventarioManagerPrueba.instancia.GetTotalADNRecolectados();

        // 2. Contamos cu�ntos ADNs f�sicos hay en la escena ahora mismo.
        int adnEnEscena = FindObjectsByType<RecolectarADN>(FindObjectsSortMode.None).Length;

        // 3. Si la suma de lo que tiene el jugador y lo que hay en escena ya es el m�ximo, no generamos m�s.
        if (adnDelJugador + adnEnEscena >= maximoADNTotal)
        {
            return;
        }

        // Si a�n no hemos llegado al l�mite, continuamos con el temporizador.
        tiempoSiguiente -= Time.deltaTime;

        if (tiempoSiguiente <= 0f)
        {
            tiempoSiguiente = intervalo;

            // Generamos uno nuevo solo si hay pocos en la escena.
            if (adnEnEscena < minimoEnEscena)
            {
                GenerarADN();
            }
        }
    }

    void GenerarADN()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-3f, 3f);
        Instantiate(prefabADN, new Vector3(x, y, 0f), Quaternion.identity);
    }
}
