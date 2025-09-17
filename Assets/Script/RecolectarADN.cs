using UnityEngine;

public class RecolectarADN : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public int quantity = 1;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (InventarioGlobal.Instance != null)
        {
            InventarioGlobal.Instance.AgregarADN(itemName, quantity);
        }
        else
        {
            Debug.LogWarning("InventarioGlobal no encontrado.");
        }

        if (audioSource != null && audioSource.clip != null)
        {
            // Reproducir sonido independiente del prefab
            AudioSource.PlayClipAtPoint(audioSource.clip, Camera.main.transform.position);
        }

        Destroy(gameObject); // ya se puede destruir de inmediato
    }
}
