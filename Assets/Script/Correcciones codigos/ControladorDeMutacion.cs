using UnityEngine;

public class ControladorDeMutacion : MonoBehaviour
{
    [Header("CONFIGURACI�N DE LA CRIATURA")]
    [Tooltip("ID �nico para guardar los datos. Ej: 'Alima�a', 'GolemDeRoca', etc.")]
    public string idUnicoCriatura;

    [Header("Requisitos de Mutaci�n")]
    public int pocionesNecesariasPrimeraMutacion = 3;
    public int pocionesNecesariasMutacionFinal = 6;

    [Header("Multiplicadores de Da�o")]
    public float multiplicadorDa�oPrimera = 1.5f;
    public float multiplicadorDa�oFinal = 2.5f;

    // --- ESTADO INTERNO (privado) ---
    private int pocionesRecibidas = 0;
    private bool yaMut�Primera = false;
    private bool yaMut�Final = false;

    // --- CLAVES PARA GUARDADO (se generan din�micamente) ---
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
        // Si no se asign� un ID, lanzamos un error para no guardar datos incorrectos.
        if (string.IsNullOrEmpty(idUnicoCriatura))
        {
            Debug.LogError("�ERROR! La criatura no tiene un 'idUnicoCriatura' asignado en el Inspector.", this.gameObject);
            return;
        }

        // Generamos las claves �nicas para PlayerPrefs
        PREF_POCIONES = idUnicoCriatura + "_Pociones";
        PREF_MUTA1 = idUnicoCriatura + "_Mutada1";
        PREF_MUTAF = idUnicoCriatura + "_MutadaFinal";

        CargarEstado();
    }

    /// <summary>
    /// Devuelve el multiplicador de da�o actual basado en el estado de mutaci�n.
    /// </summary>
    public float ObtenerMultiplicadorDa�o()
    {
        if (yaMut�Final)
            return multiplicadorDa�oFinal;
        else if (yaMut�Primera)
            return multiplicadorDa�oPrimera;
        else
            return 1f; // Da�o normal (sin multiplicar)
    }

    /// <summary>
    /// Devuelve el estado de mutaci�n actual de la criatura.
    /// </summary>
    public EstadoMutacion ObtenerEstadoMutacion()
    {
        if (yaMut�Final) return EstadoMutacion.Final;
        if (yaMut�Primera) return EstadoMutacion.Primera;
        return EstadoMutacion.Normal;
    }

    /// <summary>
    /// Llama a esta funci�n cuando la criatura recibe una poci�n.
    /// </summary>
    public void RecibirPocion()
    {
        if (yaMut�Final) return; // Si ya est� en la mutaci�n final, no hace nada.

        pocionesRecibidas++;
        Debug.Log($"?? {idUnicoCriatura} recibi� poci�n. Total: {pocionesRecibidas}");

        // Comprobamos si puede mutar
        if (pocionesRecibidas >= pocionesNecesariasMutacionFinal && !yaMut�Final && yaMut�Primera)
        {
            MutarFinal();
        }
        else if (pocionesRecibidas >= pocionesNecesariasPrimeraMutacion && !yaMut�Primera)
        {
            MutarPrimera();
        }

        GuardarEstado();
    }

    void MutarPrimera()
    {
        yaMut�Primera = true;
        pocionesRecibidas = 0; // Reiniciamos el contador para la siguiente mutaci�n
        Debug.Log($"?? �{idUnicoCriatura} ha alcanzado su PRIMERA mutaci�n!");
    }

    void MutarFinal()
    {
        yaMut�Final = true;
        Debug.Log($"?? �{idUnicoCriatura} ha alcanzado su mutaci�n FINAL!");

        // Opcional: Notificar a otros sistemas del juego
        // if (GameManager.Instance != null)
        //     GameManager.Instance.NotificarCriaturaMutadaFinal(idUnicoCriatura);
    }

    void CargarEstado()
    {
        pocionesRecibidas = PlayerPrefs.GetInt(PREF_POCIONES, 0);
        yaMut�Primera = PlayerPrefs.GetInt(PREF_MUTA1, 0) == 1;
        yaMut�Final = PlayerPrefs.GetInt(PREF_MUTAF, 0) == 1;
        Debug.Log($"Estado de {idUnicoCriatura} cargado.");
    }

    void GuardarEstado()
    {
        PlayerPrefs.SetInt(PREF_POCIONES, pocionesRecibidas);
        PlayerPrefs.SetInt(PREF_MUTA1, yaMut�Primera ? 1 : 0);
        PlayerPrefs.SetInt(PREF_MUTAF, yaMut�Final ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Funci�n de utilidad para reiniciar el estado de esta criatura durante las pruebas.
    /// </summary>
    public void ForzarReinicio()
    {
        PlayerPrefs.DeleteKey(PREF_POCIONES);
        PlayerPrefs.DeleteKey(PREF_MUTA1);
        PlayerPrefs.DeleteKey(PREF_MUTAF);
        PlayerPrefs.Save();

        pocionesRecibidas = 0;
        yaMut�Primera = false;
        yaMut�Final = false;

        Debug.Log($"?? Estado de {idUnicoCriatura} reiniciado para testing.");
    }
}
