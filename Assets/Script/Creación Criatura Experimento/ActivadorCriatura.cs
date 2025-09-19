using UnityEngine;

public class ActivadorCriatura : MonoBehaviour
{
    // Arrastra aqu� desde el Inspector el GameObject de la criatura que quieres activar.
    public GameObject objetoCriatura;

    void Start()
    {
        // 1. Busca la instancia global del manager al iniciar la escena.
        //    Esta es la "magia" del Singleton que funciona entre escenas.
        InventarioManagerPrueba manager = InventarioManagerPrueba.instancia;

        // 2. Verificamos que encontramos el manager.
        if (manager == null)
        {
            Debug.LogError("No se pudo encontrar la instancia de InventarioManagerPrueba. Aseg�rate de que tu escena inicial (donde est� el manager) se carg� primero.");
            // Desactivamos la criatura por seguridad si no encontramos el manager.
            if (objetoCriatura != null) objetoCriatura.SetActive(false);
            return;
        }

        // 3. Usamos la referencia 'manager' para decidir si activar o no la criatura.
        if (manager.criaturaCreada)
        {
            Debug.Log("La criatura ya fue creada anteriormente. Activando el objeto de la criatura.");
            if (objetoCriatura != null) objetoCriatura.SetActive(true);
        }
        else
        {
            Debug.Log("La criatura a�n no ha sido creada. El objeto de la criatura permanecer� desactivado.");
            if (objetoCriatura != null) objetoCriatura.SetActive(false);
        }
    }
}