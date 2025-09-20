using UnityEngine;


public class ActivadorCriatura : MonoBehaviour
{
    // Arrastra aquí desde el Inspector el GameObject de la criatura que quieres activar.
    public GameObject objetoCriatura;

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
        // 3. Revisamos el estado inicial al cargar la escena.
        //    Esto sirve si el jugador ya creó la criatura y vuelve a cargar esta escena.
        if (InventarioManagerPrueba.instancia != null && InventarioManagerPrueba.instancia.criaturaCreada)
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