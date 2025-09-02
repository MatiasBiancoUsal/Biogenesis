using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida; // Asignás la imagen de la barra desde el Inspector
    public Personaje personajeAsociado; // Asignás el personaje desde el Inspector

    [Header("ID de la criatura")]
    public string creatureID; // Se completa en el Inspector

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

        // ✅ Cargar vida guardada (float → int)
        personajeAsociado.vida = (int)PlayerPrefs.GetFloat("Vida_" + creatureID, vidaMaxima);

        ActualizarUI();
    }

    void Update()
    {
        if (!personajeMurio && personajeAsociado != null)
        {
            int vidaActual = Mathf.Clamp(personajeAsociado.vida, 0, vidaMaxima);
            rellenoBarraVida.fillAmount = (float)vidaActual / vidaMaxima;

            // ✅ Guardar vida constantemente (se guarda como float, aunque vida sea int)
            PlayerPrefs.SetFloat("Vida_" + creatureID, vidaActual);
            PlayerPrefs.Save();

            if (personajeAsociado.vida <= 0)
            {
                personajeMurio = true;
                rellenoBarraVida.fillAmount = 0f;
            }
        }
        else if (personajeMurio)
        {
            rellenoBarraVida.fillAmount = 0f;
        }
    }

    private void ActualizarUI()
    {
        int vidaActual = Mathf.Clamp(personajeAsociado.vida, 0, vidaMaxima);
        rellenoBarraVida.fillAmount = (float)vidaActual / vidaMaxima;
    }
}
