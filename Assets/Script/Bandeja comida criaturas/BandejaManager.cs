using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class BandejaManager : MonoBehaviour
{
    [Header("UI de la Bandeja")]
    public GameObject bandejaPanel;   // El panel principal de la bandeja
    public Transform content;         // Donde se ponen las comidas (UI)

    [Header("Prefabs de comida UI")]
    public GameObject[] comidaPrefabs; // Prefabs visuales (UI) de comida

    [Header("Prefabs físicos para la escena")]
    public GameObject[] comidaFisicaPrefabs; // Prefabs reales con Collider2D y tag "Food"

    [Header("Configuración")]
    public int maxComidas = 3;

    private int comidasActuales = 0;

    void Start()
    {
        // bandejaPanel.SetActive(false); // si querés ocultarla al inicio
    }

    public void ToggleBandeja()
    {
        bandejaPanel.SetActive(!bandejaPanel.activeSelf);
    }

    public void AgregarComida(int index)
    {
        if (comidasActuales >= maxComidas) return;

        Debug.Log($"Instanciando prefab UI: {comidaPrefabs[index].name}");

        Instantiate(comidaPrefabs[index], content); // solo visual
        comidasActuales++;

        // Instanciar versión física también
        SoltarComidaEnEscena(index);

        CustomEvent comidas = new CustomEvent("comida_transportada")
                {};
        //AnalyticsService.Instance.RecordEvent("comida_transportada");
        print("evento " + "comida_escenas " + index);
        AnalyticsService.Instance.Flush();
    }

    public void SoltarComidaEnEscena(int index)
    {
        if (index < 0 || index >= comidaFisicaPrefabs.Length) return;

        Vector3 posicion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicion.z = 0f;

        GameObject comida = Instantiate(comidaFisicaPrefabs[index], posicion, Quaternion.identity);
        comida.tag = "Food"; // Asegurar el tag correcto

        Debug.Log($"Instanciada comida física: {comida.name} en {posicion}");
    }

    public void QuitarComida()
    {
        if (comidasActuales > 0)
            comidasActuales--;
    }
}
