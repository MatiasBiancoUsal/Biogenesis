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
    [Tooltip("Configura aqu� todos los tipos de ADN que existen en el juego.")]
    public DefinicionADN[] databaseADN;

    [Header("Datos del Inventario")]
    [Tooltip("Estado actual del inventario del jugador.")]
    public Slot[] slots;

    [Header("L�gica de la Criatura")]
    [Tooltip("Esta bandera se vuelve 'true' cuando la criatura ha sido creada.")]
    public bool criaturaCreada = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("--- PRUEBA DE TECLADO: Forzando creaci�n de criatura... ---");
            CrearCriatura();
        }
    }
    /// <summary>
    /// A�ade un ADN al inventario y notifica a la UI.
    /// </summary>
    public void A�adirADN(string nombreADN)
    {
        foreach (Slot slot in slots)
        {
            if (slot.nombreADN == nombreADN)
            {
                slot.cantidad++;
                OnInventarioChanged?.Invoke(); // Avisa a la UI que se actualice.
                return; // Sale de la funci�n una vez que encuentra y actualiza el item.
            }
        }
    }

    /// <summary>
    /// Este m�todo es llamado por el bot�n de la UI.
    /// </summary>
    public void BotonPresionadoCrearCriatura()
    {
        if (TodosLosADNRecolectados() && !criaturaCreada)
        {
            Debug.Log("Bot�n presionado. Marcando criatura como 'creada'.");
            CrearCriatura();
        }
    }

    /// <summary>
    /// Pone a cero todas las cantidades del inventario y resetea el estado de la criatura.
    /// </summary>
    public void ReiniciarInventario()
    {
        foreach (Slot slot in slots)
        {
            slot.cantidad = 0;
        }
        criaturaCreada = false;
        OnInventarioChanged?.Invoke(); // Avisa a la UI para que se actualice a ceros.
        Debug.Log("�Inventario Reiniciado!");
    }

    /// <summary>
    /// Revisa si el jugador ha recolectado 4 o m�s de cada tipo de ADN.
    /// </summary>
    public bool TodosLosADNRecolectados()
    {
        foreach (Slot slot in slots)
        {
            if (slot.cantidad < 4) return false;
        }
        return true;
    }

    /// <summary>
    /// Devuelve el Sprite de un ADN busc�ndolo por su nombre en la base de datos.
    /// </summary>
    public Sprite GetIconoPorNombre(string nombreADN)
    {
        foreach (DefinicionADN definicion in databaseADN)
        {
            if (definicion.nombre == nombreADN) return definicion.icono;
        }
        return null;
    }

    /// <summary>
    /// Contiene la l�gica a ejecutar una vez que se crea la criatura.
    /// </summary>
    private void CrearCriatura()
    {
        criaturaCreada = true;
        OnCriaturaCreada?.Invoke(); // Avisa al "pantallazo" que debe mostrarse.
        Debug.Log("La bandera 'criaturaCreada' es ahora true.");

        // A�ADE ESTA L�NEA:
        OnInventarioChanged?.Invoke(); // Notifica a la UI que debe re-evaluar su estado.
    }
}