using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADNAlimaña : MonoBehaviour
{
    public GameObject prefabADN;
    public float intervalo = 5f;

    [Header("Límites de Generación")]
    public int maximoADNTotal = 9; // El máximo que el jugador puede tener en su inventario.
    public int minimoEnEscena = 4; // Intentará que siempre haya este número disponible.

    private float tiempoSiguiente;

    void Start()
    {
        tiempoSiguiente = intervalo;
    }

    void Update()
    {
        if (InventarioManagerPrueba.instancia == null) return; // Esperar a que el inventario esté listo

        // 1. Le preguntamos al inventario el total de ADN que ya tiene el jugador.
        int adnDelJugador = InventarioManagerPrueba.instancia.GetTotalADNRecolectados();

        // 2. Contamos cuántos ADNs físicos hay en la escena ahora mismo.
        int adnEnEscena = FindObjectsByType<RecolectarADN>(FindObjectsSortMode.None).Length;

        // 3. Si la suma de lo que tiene el jugador y lo que hay en escena ya es el máximo, no generamos más.
        if (adnDelJugador + adnEnEscena >= maximoADNTotal)
        {
            return;
        }

        // Si aún no hemos llegado al límite, continuamos con el temporizador.
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
