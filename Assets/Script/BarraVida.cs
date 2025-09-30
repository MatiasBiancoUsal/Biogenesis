using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;          // La imagen de la barra desde el Inspector
    public Personaje personajeAsociado;     // Script de la criatura (debe tener vida y vidaMaxima)

    [Header("ID de la criatura")]
    public string creatureID; // "Araña", "Alimaña", "Mutante", "Cazador"

    private int vidaMaxima;
    private bool personajeMurio = false;

    void Start()
    {
        if (personajeAsociado == null)
        {
            Debug.LogError("No se asignó un personaje a esta barra de vida.");
            return;
        }

        vidaMaxima = personajeAsociado.vidaMaxima;

        // ✅ Cargar vida guardada desde PlayerPrefs
        personajeAsociado.vida = PlayerPrefs.GetInt("Vida_" + creatureID, vidaMaxima);

        ActualizarUI();
    }

    void Update()
    {
        if (personajeMurio || personajeAsociado == null) return;

        int vidaActual = Mathf.Clamp(personajeAsociado.vida, 0, vidaMaxima);

        // ✅ Actualizar barra local
        rellenoBarraVida.fillAmount = (float)vidaActual / vidaMaxima;

        // ✅ Guardar en PlayerPrefs
        PlayerPrefs.SetInt("Vida_" + creatureID, vidaActual);

        // ✅ Actualizar estado global
        if (EstadoCriaturasGlobal.instancia != null)
        {
            switch (creatureID)
            {
                case "Araña": EstadoCriaturasGlobal.instancia.vidaAraña = vidaActual; break;
                case "Alimaña": EstadoCriaturasGlobal.instancia.vidaAlimaña = vidaActual; break;
                case "Mutante": EstadoCriaturasGlobal.instancia.vidaMutante = vidaActual; break;
                case "Cazador": EstadoCriaturasGlobal.instancia.vidaCazador = vidaActual; break;
            }

            EstadoCriaturasGlobal.instancia.GuardarEstado();
        }

        // ✅ Revisar muerte
        if (personajeAsociado.vida <= 0)
        {
            personajeMurio = true;
            rellenoBarraVida.fillAmount = 0f;
        }
    }

    private void ActualizarUI()
    {
        int vidaActual = Mathf.Clamp(personajeAsociado.vida, 0, vidaMaxima);
        rellenoBarraVida.fillAmount = (float)vidaActual / vidaMaxima;
    }
}
