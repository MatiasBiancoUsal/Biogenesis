using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using Unity.Services.Analytics;

public class MiniJuegoManager : MonoBehaviour
{
    public static MiniJuegoManager Instance;

    private Dictionary<ConectorScript.ColorCable, bool> conexiones = new();

    public GameObject cableRojo;
    public GameObject cableVerde;
    public GameObject cableAzul;

    private void Awake()
    {
        Instance = this;
        ResetearConexiones();

        // Asegurate de que los cables estén ocultos al comenzar
        cableRojo.SetActive(false);
        cableVerde.SetActive(false);
        cableAzul.SetActive(false);
    }

    public void ConectarCables(ConectorScript.ColorCable color, ConectorScript a, ConectorScript b)
    {
        // Aquí deberías dibujar visualmente el cable (LineRenderer o sprite)
        conexiones[color] = true;

        switch (color)
        {
            case ConectorScript.ColorCable.Rojo:
                cableRojo.SetActive(true);
                break;
            case ConectorScript.ColorCable.Verde:
                cableVerde.SetActive(true);
                break;
            case ConectorScript.ColorCable.Azul:
                cableAzul.SetActive(true);
                break;
        }

        VerificarCompleto();
    }

    void VerificarCompleto()
    {
        if (conexiones.Values.All(conectado => conectado))
        {
            Debug.Log("¡Mini juego completado!");
            PlayerPrefs.SetInt("MiniJuegoResuelto", 1);
            SceneManager.LoadScene("Laboratorio");
        }

        CustomEvent verificarcompleto = new CustomEvent("minijuego_completado")
                {
                    { "minijuego_completo", true}
                };
        //AnalyticsService.Instance.RecordEvent(verificarcompleto);
        print("evento " + "minijuego_completado " + true);
        AnalyticsService.Instance.Flush();
        //
    }

    public void ResetearConexiones()
    {
        conexiones[ConectorScript.ColorCable.Rojo] = false;
        conexiones[ConectorScript.ColorCable.Verde] = false;
        conexiones[ConectorScript.ColorCable.Azul] = false;
    }
}
