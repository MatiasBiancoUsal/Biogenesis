using UnityEngine;

public class RecolectarADN : MonoBehaviour
{
    public string itemName;
    public int quantity = 1;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // --- OPCIONAL PERO RECOMENDADO ---
        // Si el objeto de ADN se crea (ej. un enemigo lo suelta)
        // y la criatura YA existe, simplemente se destruye al nacer.
        if (InventarioManagerPrueba.instancia != null && InventarioManagerPrueba.instancia.criaturaCreada)
        {
            Destroy(gameObject);
            return;
        }
        // --- Fin Opcional ---
    }

    void OnMouseDown()
    {
        // --- 1. PRIMERA VERIFICACIÓN ---
        // Nos aseguramos de que el inventario exista
        if (InventarioManagerPrueba.instancia == null)
        {
            Debug.LogError("ERROR CRÍTICO: No se encuentra la instancia de InventarioManagerPrueba.");
            return; // Salimos si no hay inventario
        }

        // --- 2. LÍNEA AÑADIDA (LA CLAVE) ---
        // Si la criatura ya fue creada, salimos de la función INMEDIATAMENTE.
        // No se añade ADN, no suena, y no se destruye. Se vuelve "no-clicable".
        if (InventarioManagerPrueba.instancia.criaturaCreada)
        {
            return;
        }
        // --- FIN DE LA MODIFICACIÓN ---

        // Si llegamos aquí, es porque la criatura NO ha sido creada y podemos recolectar.
        InventarioManagerPrueba.instancia.AñadirADN(itemName);

        if (audioSource != null && audioSource.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, Camera.main.transform.position);
        }

        Destroy(gameObject);
    }
}