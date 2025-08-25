using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandejaManager : MonoBehaviour
{
    [Header("UI de la Bandeja")]
    public GameObject bandejaPanel;   // El panel principal de la bandeja
    public Transform content;         // Donde se ponen las comidas (Horizontal Layout Group)

    [Header("Prefabs de comida UI")]
    public GameObject[] comidaPrefabs; // Prefabs de las 5 comidas en versi�n UI

    [Header("Configuraci�n")]
    public int maxComidas = 3;         // M�ximo de comidas permitidas

    private int comidasActuales = 0;

    void Start()
    {
        // Al inicio, la bandeja est� cerrada
        bandejaPanel.SetActive(false);
    }

    public void ToggleBandeja()
    {
        bandejaPanel.SetActive(!bandejaPanel.activeSelf);
    }

    public void AgregarComida(int index)
    {
        // No dejar meter m�s de 3
        if (comidasActuales >= maxComidas) return;

        Debug.Log($"Instanciando prefab: {comidaPrefabs[index].name} (index {index})");

        // Instanciar la comida UI en el contenedor
        Instantiate(comidaPrefabs[index], content);
        comidasActuales++;
    }
}
