using UnityEngine;

public class RecolectarADN : MonoBehaviour
{
    public string itemName;
   
    public int quantity = 1;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (InventarioManagerPrueba.instancia != null)
        {
            InventarioManagerPrueba.instancia.AñadirADN(itemName);
        }
        else
        {
            Debug.LogError("ERROR CRÍTICO: No se encuentra la instancia de InventarioManagerPrueba. Asegúrate de que el objeto con este script esté en tu primera escena (Laboratorio).");
        }

        if (audioSource != null && audioSource.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, Camera.main.transform.position);
        }

        Destroy(gameObject);
    }
}
