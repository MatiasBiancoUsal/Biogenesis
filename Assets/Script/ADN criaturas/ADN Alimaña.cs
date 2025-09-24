using UnityEngine;

public class ADNAlimaña : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject prefabADN;
    public float intervalo = 5f;

    [Header("Límites de Generación")]
    public int maximoPorTipo = 4;
    public int minimoEnEscena = 2;

    private float tiempoSiguiente;
    private string nombreDeEsteADN;

    void Start()
    {
        tiempoSiguiente = intervalo;
        if (prefabADN != null && prefabADN.GetComponent<RecolectarADN>() != null)
        {
            nombreDeEsteADN = prefabADN.GetComponent<RecolectarADN>().itemName;
        }
        else
        {
            Debug.LogError($"El prefab en '{gameObject.name}' no tiene el script 'RecolectarADN' o no tiene un itemName.", this);
        }
    }

    void Update()
    {
        if (string.IsNullOrEmpty(nombreDeEsteADN) || InventarioManagerPrueba.instancia == null) return;

        // Pregunta por la cantidad de su tipo específico de ADN
        int adnDelJugador = InventarioManagerPrueba.instancia.GetCantidadDeADN(nombreDeEsteADN);

        // Cuenta solo los objetos en escena de su tipo específico
        int adnEnEscena = 0;
        RecolectarADN[] todosLosAdnEnEscena = FindObjectsByType<RecolectarADN>(FindObjectsSortMode.None);
        foreach (var adn in todosLosAdnEnEscena)
        {
            if (adn.itemName == nombreDeEsteADN)
            {
                adnEnEscena++;
            }
        }

        if (adnDelJugador + adnEnEscena >= maximoPorTipo) return;

        tiempoSiguiente -= Time.deltaTime;

        if (tiempoSiguiente <= 0f)
        {
            tiempoSiguiente = intervalo;
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