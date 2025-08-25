using UnityEngine;

public class BolsaComida : MonoBehaviour
{
    public int comidaIndex; // Qué comida es (ej: 0 = araña, 1 = rata...)
    private BandejaManager bandejaManager;

    void Start()
    {
        bandejaManager = FindFirstObjectByType<BandejaManager>();
    }

    void OnMouseDown()
    {
        if (bandejaManager != null)
        {
            Debug.Log($"Click detectado en: {gameObject.name} con index {comidaIndex}");
            bandejaManager.AgregarComida(comidaIndex);
        }
    }
}