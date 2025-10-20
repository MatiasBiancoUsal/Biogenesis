using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Services.Analytics;
using UnityEngine;

public class MaletinManager : MonoBehaviour
{
    public static MaletinManager instancia;

    public string pocionVidaID;
    public string pocionMejoraID;

    public bool tienePocionVida()
    {
        return !string.IsNullOrEmpty(pocionVidaID);
    }

    public bool tienePocionMejora()
    {
        return !string.IsNullOrEmpty(pocionMejoraID);
    }

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        // Solo la primera vez se inicializa vacío
        if (!string.IsNullOrEmpty(pocionVidaID) || !string.IsNullOrEmpty(pocionMejoraID))
        {
            pocionVidaID = null;
            pocionMejoraID = null;
        }
    }

    public void GuardarPocion(GameObject prefabPocion)
    {
        if (prefabPocion.CompareTag("PocionVida"))
        {
            pocionVidaID = prefabPocion.name;
            Debug.Log("Guardado ID: " + pocionVidaID);
        }
        else if (prefabPocion.CompareTag("PocionMejora"))
        {
            pocionMejoraID = prefabPocion.name;
            Debug.Log("Guardado ID: " + pocionMejoraID);
        }

        CustomEvent pociones = new CustomEvent("pociones_transportadas")
        { };
        AnalyticsService.Instance.RecordEvent("transporte_pociones");
        print("evento " + "pociones_transportadas ");
        AnalyticsService.Instance.Flush();
    }

    // SOO
    public void QuitarPocion(string tipoPocionTag)
    {
        if (tipoPocionTag == "PocionVida")
        {
            pocionVidaID = null;
        }
        else if (tipoPocionTag == "PocionMejora")
        {
            pocionMejoraID = null;
        }

       
    }
    // SOO

    public List<string> ObtenerPocionesID()
    {
        var lista = new List<string>();
        if (tienePocionVida()) lista.Add(pocionVidaID);
        if (tienePocionMejora()) lista.Add(pocionMejoraID);
        return lista;
    }
}