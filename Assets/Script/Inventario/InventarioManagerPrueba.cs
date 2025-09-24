using UnityEngine;
using System;

[System.Serializable]
public class DefinicionADN
{
    public string nombre;
    public Sprite icono;
}

[System.Serializable]
public class Slot
{
    public string nombreADN;
    public int cantidad;
}

public class InventarioManagerPrueba : MonoBehaviour
{
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
        criaturaCreada = PlayerPrefs.GetInt(KEY_CRIATURA_CREADA, 0) == 1;
        
        if (criaturaCreada)
        {
            OnCriaturaCreada?.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("--- PRUEBA DE TECLADO: Forzando creaci�n de criatura... ---");
            CrearCriatura();
        }
    }

    public void A�adirADN(string nombreADN)
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
            Debug.Log("Bot�n presionado. Marcando criatura como 'creada'.");
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
        Debug.Log("�Inventario Reiniciado!");
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
        PlayerPrefs.SetInt(KEY_CRIATURA_CREADA, 1);
        PlayerPrefs.Save(); 
        OnInventarioChanged?.Invoke();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotificarExperimentoCreado();
        }
    }

    public int GetCantidadDeADN(string nombreADN)
    {
        foreach (Slot slot in slots)
        {
            if (slot.nombreADN == nombreADN)
            {
                return slot.cantidad;
            }
        }
        return 0;
    }
}