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
    // --- SINGLETON Y EVENTOS ---
    public static InventarioManagerPrueba instancia;
    public static event Action OnInventarioChanged;
    public static event Action OnCriaturaCreada;

    [Header("Base de Datos de Items")]
    [Tooltip("Configura aquí todos los tipos de ADN que existen en el juego.")]
    public DefinicionADN[] databaseADN;

    [Header("Datos del Inventario")]
    [Tooltip("Estado actual del inventario del jugador.")]
    public Slot[] slots;

    [Header("Lógica de la Criatura")]
    [Tooltip("Esta bandera se vuelve 'true' cuando la criatura ha sido creada.")]
    public bool criaturaCreada = false;

    // Constante para la clave de guardado en PlayerPrefs
    private const string KEY_CRIATURA_CREADA = "CriaturaCreada";

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

    void Start()
    {
        //  PASO 1: Cargamos el estado de la criatura al iniciar la escena.
        // Si existe la clave en PlayerPrefs, cargamos su valor (0 = false, 1 = true).
        criaturaCreada = PlayerPrefs.GetInt(KEY_CRIATURA_CREADA, 0) == 1;

        // Si la criatura ya estaba creada, la activamos.
        if (criaturaCreada)
        {
            OnCriaturaCreada?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("--- PRUEBA DE TECLADO: Forzando creación de criatura... ---");
            CrearCriatura();
        }
    }

    public void AñadirADN(string nombreADN)
    {
        foreach (Slot slot in slots)
        {
            if (slot.nombreADN == nombreADN)
            {
                slot.cantidad++;
                OnInventarioChanged?.Invoke();
                return;
            }
        }
    }

    public void BotonPresionadoCrearCriatura()
    {
        if (TodosLosADNRecolectados() && !criaturaCreada)
        {
            Debug.Log("Botón presionado. Marcando criatura como 'creada'.");
            CrearCriatura();
        }
    }

    public void ReiniciarInventario()
    {
        foreach (Slot slot in slots)
        {
            slot.cantidad = 0;
        }
        criaturaCreada = false;
        OnInventarioChanged?.Invoke();
        Debug.Log("¡Inventario Reiniciado!");
    }

    public bool TodosLosADNRecolectados()
    {
        foreach (Slot slot in slots)
        {
            if (slot.cantidad < 4) return false;
        }
        return true;
    }

    public Sprite GetIconoPorNombre(string nombreADN)
    {
        foreach (DefinicionADN definicion in databaseADN)
        {
            if (definicion.nombre == nombreADN) return definicion.icono;
        }
        return null;
    }

    private void CrearCriatura()
    {
        criaturaCreada = true;
        OnCriaturaCreada?.Invoke();
        Debug.Log("La bandera 'criaturaCreada' es ahora true.");

        //  PASO 2: Guardamos el estado en PlayerPrefs. Usamos 1 para true.
        PlayerPrefs.SetInt(KEY_CRIATURA_CREADA, 1);
        PlayerPrefs.Save(); // Forzamos el guardado en el disco.

        OnInventarioChanged?.Invoke();
    }
}