using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mapa : MonoBehaviour
{
    public void escenas (int numeroDeEscena)
      
    {

        CustomEvent derrota = new CustomEvent("escena_ingresada")
        {
            { "ingreso_escena", numeroDeEscena}
        };
        AnalyticsService.Instance.RecordEvent(derrota);
        print("evento " + "escena_ingresada " + numeroDeEscena);



                print("evento " + "continuarpartida ");
        AnalyticsService.Instance.Flush();

        SceneManager.LoadScene(numeroDeEscena);
    }
}
