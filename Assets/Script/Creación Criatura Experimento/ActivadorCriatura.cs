using UnityEngine;

public class ActivadorCriatura : MonoBehaviour
{
    // Arrastra aquí desde el Inspector el GameObject de la criatura que quieres activar.
    public GameObject objetoCriatura;

    // Constante para la clave de guardado en PlayerPrefs
    private const string KEY_CRIATURA_CREADA = "CriaturaCreada";

    // --- CICLO DE VIDA DE UNITY ---

    void OnEnable()
    {
        // 1. Nos suscribimos al evento. Ahora este script será notificado.
        InventarioManagerPrueba.OnCriaturaCreada += ActivarLaCriatura;
    }

    void OnDisable()
    {
        // 2. Nos desuscribimos para evitar errores. ¡Buena práctica!
        InventarioManagerPrueba.OnCriaturaCreada -= ActivarLaCriatura;
    }

    void Start()
    {
        //  PASO 1: Revisa el estado guardado al cargar la escena.
        // Si la criatura ya estaba creada en una partida anterior, la activamos.
        // Usamos PlayerPrefs para verificar si la clave existe y su valor es 1 (true).
        if (PlayerPrefs.GetInt(KEY_CRIATURA_CREADA, 0) == 1)
        {
            ActivarLaCriatura();
        }
        else
        {
            // Nos aseguramos de que esté desactivada al empezar.
            if (objetoCriatura != null) objetoCriatura.SetActive(false);
        }
    }

    // --- MÉTODOS ---

    /// <summary>
    /// Este método es llamado por el evento OnCriaturaCreada.
    /// </summary>
    private void ActivarLaCriatura()
    {
        Debug.Log("¡Evento OnCriaturaCreada recibido! Activando la criatura.");
        if (objetoCriatura != null)
        {
            objetoCriatura.SetActive(true);
        }
    }
}