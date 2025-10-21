using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarBarrasVida : MonoBehaviour
{
    public Image barraAra�a;
    public Image barraAlima�a;
    public Image barraMutante;
    public Image barraCazador;
    // Debes a�adir aqu� la barra del "Experimento" si la tienes.
    // public Image barraExperimento; 

    void Start()
    {
        //  CAMBIO CLAVE:
        // Al cargar la escena, forzamos la instancia global a leer los �ltimos
        // datos de vida guardados en PlayerPrefs.
        if (EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.CargarEstado();
        }
    }

    void Update()
    {
        if (EstadoCriaturasGlobal.instancia == null)
            Debug.LogError("EstadoCriaturasGlobal no encontrado!");
        return;

        // El Update ahora muestra los valores reci�n cargados.
        barraAra�a.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAra�a / EstadoCriaturasGlobal.instancia.vidaMaxAra�a);
        barraAlima�a.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAlima�a / EstadoCriaturasGlobal.instancia.vidaMaxAlima�a);
        barraMutante.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaMutante / EstadoCriaturasGlobal.instancia.vidaMaxMutante);
        barraCazador.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaCazador / EstadoCriaturasGlobal.instancia.vidaMaxCazador);
        Debug.Log("Vida Ara�a (Global): " + EstadoCriaturasGlobal.instancia.vidaAra�a);
        // Si a�adiste Experimento:
        //barraExperimento.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaExperimento / EstadoCriaturasGlobal.instancia.vidaMaxExperimento);
    }
}