using UnityEngine;
using System;

// --- ESTRUCTURA PARA NUESTRA "BASE DE DATOS" DE ITEMS ---
[System.Serializable]
public class DefinicionADN
{
    public string nombre;
    public Sprite icono;
}

// --- CLASE DE DATOS PARA EL INVENTARIO EN CURSO ---
[System.Serializable]
public class Slot
{
    public string nombreADN;
    public int cantidad;
}

public class InventarioManagerPrueba : MonoBehaviour
{
    public static InventarioManagerPrueba instancia;

    [Header("Base de Datos de Items")]
    [Tooltip("Configura aquí todos los tipos de ADN que existen en el juego.")]
    public DefinicionADN[] databaseADN;

    [Header("Datos del Inventario")]
    [Tooltip("Estado actual del inventario del jugador.")]
    public Slot[] slots;

    [Header("Lógica de la Criatura")]
    public GameObject criaturaExperimentoPrefab;
    public Transform spawnPoint;
    public bool criaturaCreada = false;
    [Tooltip("Arrastra aquí el botón de la UI para crear la criatura.")]
    public GameObject botonCrearCriatura;
    // Evento para notificar a la UI de los cambios
    public static event Action OnInventarioChanged;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // --- MÉTODO MODIFICADO ---
    // Ahora solo necesita el nombre, es más simple y robusto.
    public void AñadirADN(string nombreADN)
    {
        bool adnEncontrado = false;
        foreach (Slot slot in slots)
        {
            if (slot.nombreADN == nombreADN)
            {
                slot.cantidad++;
                adnEncontrado = true;
                Debug.Log($"DATO AÑADIDO: {nombreADN}, cantidad ahora {slot.cantidad}");

                // Avisamos que el inventario cambió
                OnInventarioChanged?.Invoke();
                break;
            }
        }

        if (!adnEncontrado)
        {
            Debug.LogWarning($"AVISO: Se intentó añadir '{nombreADN}', pero no se encontró un slot con ese nombre en 'Datos del Inventario'.");
        }

        RevisarADNCompletos();
    }

    // --- NUEVA FUNCIÓN ---
    // Permite que la UI (y otros scripts) pregunten por el ícono correcto.
    public Sprite GetIconoPorNombre(string nombreADN)
    {
        foreach (DefinicionADN definicion in databaseADN)
        {
            if (definicion.nombre == nombreADN)
            {
                return definicion.icono;
            }
        }
        Debug.LogWarning($"No se encontró un ícono para '{nombreADN}' en la base de datos.");
        return null;
    }

    // --- LÓGICA DE JUEGO (sin cambios) ---
    private void RevisarADNCompletos()
    {
        if (TodosLosADNRecolectados())
        {
            if (!criaturaCreada)
            {
                InstanciarCriatura();
                CrearCriatura();
            }
        }
    }

    bool TodosLosADNRecolectados()
    {
        foreach (Slot slot in slots)
        {
            if (slot.cantidad < 4)
                return false;
        }
        return true;
    }

    void CrearCriatura()
    {
        Debug.Log("¡Lógica de Criatura Creada completada!");
        criaturaCreada = true;
        // GameManager.Instance.NotificarExperimentoCreado(); // Si usas un GameManager

        //Script Sofi para bloquear y desbloquear la escena de la criatura experimento
        BotonCriaturaExperimento botonExp = FindAnyObjectByType<BotonCriaturaExperimento>();

        if (botonExp != null)
        {
            botonExp.ActualizarEstadoBoton();
            Debug.Log("Botón de criatura Experimento actualizado.");
        }
        //
    }

    private void InstanciarCriatura()
    {
        if (criaturaExperimentoPrefab != null && spawnPoint != null)
        {
            Instantiate(criaturaExperimentoPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("¡Criatura Experimento instanciada en la escena!");
        }
    }
}