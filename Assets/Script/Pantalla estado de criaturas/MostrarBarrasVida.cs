using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarBarrasVida : MonoBehaviour
{
    public Image barraAraña;
    public Image barraAlimaña;
    public Image barraMutante;
    public Image barraCazador;
    // Debes añadir aquí la barra del "Experimento" si la tienes.
    // public Image barraExperimento; 

    void Start()
    {
        //  CAMBIO CLAVE:
        // Al cargar la escena, forzamos la instancia global a leer los últimos
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

        // El Update ahora muestra los valores recién cargados.
        barraAraña.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAraña / EstadoCriaturasGlobal.instancia.vidaMaxAraña);
        barraAlimaña.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAlimaña / EstadoCriaturasGlobal.instancia.vidaMaxAlimaña);
        barraMutante.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaMutante / EstadoCriaturasGlobal.instancia.vidaMaxMutante);
        barraCazador.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaCazador / EstadoCriaturasGlobal.instancia.vidaMaxCazador);
        Debug.Log("Vida Araña (Global): " + EstadoCriaturasGlobal.instancia.vidaAraña);
        // Si añadiste Experimento:
        //barraExperimento.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaExperimento / EstadoCriaturasGlobal.instancia.vidaMaxExperimento);
    }
}