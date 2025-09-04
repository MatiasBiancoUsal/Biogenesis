using UnityEngine;

public class ControladorDeMutacion : MonoBehaviour
{
    [Header("CONFIGURACIÓN DE LA CRIATURA")]
    [Tooltip("ID único para guardar los datos. Ej: 'Alimaña', 'GolemDeRoca', etc.")]
    public string idUnicoCriatura;

    [Header("Requisitos de Mutación")]
    public int pocionesNecesariasPrimeraMutacion = 3;
    public int pocionesNecesariasMutacionFinal = 6;

    [Header("Multiplicadores de Daño")]
    public float multiplicadorDañoPrimera = 1.5f;
    public float multiplicadorDañoFinal = 2.5f;

    // --- ESTADO INTERNO (privado) ---
    private int pocionesRecibidas = 0;
    private bool yaMutóPrimera = false;
    private bool yaMutóFinal = false;

    // --- CLAVES PARA GUARDADO (se generan dinámicamente) ---
    private string PREF_POCIONES;
    private string PREF_MUTA1;
    private string PREF_MUTAF;

    public enum EstadoMutacion
    {
        Normal,
        Primera,
        Final
    }

    void Awake()
    {
        // Si no se asignó un ID, lanzamos un error para no guardar datos incorrectos.
        if (string.IsNullOrEmpty(idUnicoCriatura))
        {
            Debug.LogError("¡ERROR! La criatura no tiene un 'idUnicoCriatura' asignado en el Inspector.", this.gameObject);
            return;
        }

        // Generamos las claves únicas para PlayerPrefs
        PREF_POCIONES = idUnicoCriatura + "_Pociones";
        PREF_MUTA1 = idUnicoCriatura + "_Mutada1";
        PREF_MUTAF = idUnicoCriatura + "_MutadaFinal";

        CargarEstado();
    }

    /// <summary>
    /// Devuelve el multiplicador de daño actual basado en el estado de mutación.
    /// </summary>
    public float ObtenerMultiplicadorDaño()
    {
        if (yaMutóFinal)
            return multiplicadorDañoFinal;
        else if (yaMutóPrimera)
            return multiplicadorDañoPrimera;
        else
            return 1f; // Daño normal (sin multiplicar)
    }

    /// <summary>
    /// Devuelve el estado de mutación actual de la criatura.
    /// </summary>
    public EstadoMutacion ObtenerEstadoMutacion()
    {
        if (yaMutóFinal) return EstadoMutacion.Final;
        if (yaMutóPrimera) return EstadoMutacion.Primera;
        return EstadoMutacion.Normal;
    }

    /// <summary>
    /// Llama a esta función cuando la criatura recibe una poción.
    /// </summary>
    public void RecibirPocion()
    {
        if (yaMutóFinal) return; // Si ya está en la mutación final, no hace nada.

        pocionesRecibidas++;
        Debug.Log($"?? {idUnicoCriatura} recibió poción. Total: {pocionesRecibidas}");

        // Comprobamos si puede mutar
        if (pocionesRecibidas >= pocionesNecesariasMutacionFinal && !yaMutóFinal && yaMutóPrimera)
        {
            MutarFinal();
        }
        else if (pocionesRecibidas >= pocionesNecesariasPrimeraMutacion && !yaMutóPrimera)
        {
            MutarPrimera();
        }

        GuardarEstado();
    }

    void MutarPrimera()
    {
        yaMutóPrimera = true;
        pocionesRecibidas = 0; // Reiniciamos el contador para la siguiente mutación
        Debug.Log($"?? ¡{idUnicoCriatura} ha alcanzado su PRIMERA mutación!");
    }

    void MutarFinal()
    {
        yaMutóFinal = true;
        Debug.Log($"?? ¡{idUnicoCriatura} ha alcanzado su mutación FINAL!");

        // Opcional: Notificar a otros sistemas del juego
        // if (GameManager.Instance != null)
        //     GameManager.Instance.NotificarCriaturaMutadaFinal(idUnicoCriatura);
    }

    void CargarEstado()
    {
        pocionesRecibidas = PlayerPrefs.GetInt(PREF_POCIONES, 0);
        yaMutóPrimera = PlayerPrefs.GetInt(PREF_MUTA1, 0) == 1;
        yaMutóFinal = PlayerPrefs.GetInt(PREF_MUTAF, 0) == 1;
        Debug.Log($"Estado de {idUnicoCriatura} cargado.");
    }

    void GuardarEstado()
    {
        PlayerPrefs.SetInt(PREF_POCIONES, pocionesRecibidas);
        PlayerPrefs.SetInt(PREF_MUTA1, yaMutóPrimera ? 1 : 0);
        PlayerPrefs.SetInt(PREF_MUTAF, yaMutóFinal ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Función de utilidad para reiniciar el estado de esta criatura durante las pruebas.
    /// </summary>
    public void ForzarReinicio()
    {
        PlayerPrefs.DeleteKey(PREF_POCIONES);
        PlayerPrefs.DeleteKey(PREF_MUTA1);
        PlayerPrefs.DeleteKey(PREF_MUTAF);
        PlayerPrefs.Save();

        pocionesRecibidas = 0;
        yaMutóPrimera = false;
        yaMutóFinal = false;

        Debug.Log($"?? Estado de {idUnicoCriatura} reiniciado para testing.");
    }
}
