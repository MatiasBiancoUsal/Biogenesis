using UnityEngine;


public class ActivadorCriatura : MonoBehaviour
{
    // Arrastra aqu� desde el Inspector el GameObject de la criatura que quieres activar.
    public GameObject objetoCriatura;

    // --- CICLO DE VIDA DE UNITY ---

    void OnEnable()
    {
        // 1. Nos suscribimos al evento. Ahora este script ser� notificado.
        InventarioManagerPrueba.OnCriaturaCreada += ActivarLaCriatura;
    }

    void OnDisable()
    {
        // 2. Nos desuscribimos para evitar errores. �Buena pr�ctica!
        InventarioManagerPrueba.OnCriaturaCreada -= ActivarLaCriatura;
    }

    void Start()
    {
        // 3. Revisamos el estado inicial al cargar la escena.
        //    Esto sirve si el jugador ya cre� la criatura y vuelve a cargar esta escena.
        if (InventarioManagerPrueba.instancia != null && InventarioManagerPrueba.instancia.criaturaCreada)
        {
            ActivarLaCriatura();
        }
        else
        {
            // Nos aseguramos de que est� desactivada al empezar.
            if (objetoCriatura != null) objetoCriatura.SetActive(false);
        }
    }

    // --- M�TODOS ---

    /// <summary>
    /// Este m�todo es llamado por el evento OnCriaturaCreada.
    /// </summary>
    private void ActivarLaCriatura()
    {
        Debug.Log("�Evento OnCriaturaCreada recibido! Activando la criatura.");
        if (objetoCriatura != null)
        {
            objetoCriatura.SetActive(true);
        }
    }
}