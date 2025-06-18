using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarraVida : MonoBehaviour
{

    public Image rellenoBarraVida; // Asignás la imagen de la barra desde el Inspector
    public Personaje personajeAsociado; // Asignás el personaje desde el Inspector

    private float vidaMaxima;
    private bool personajeMurio = false;


    // Start is called before the first frame update
    void Start()
    {
        if (personajeAsociado == null)
        {
            Debug.LogError(" No se asignó un personaje a esta barra de vida.");
            return;
        }

        vidaMaxima = personajeAsociado.vidaMaxima;
    }

    // Update is called once per frame
    void Update()
    {
        if (!personajeMurio && personajeAsociado != null)
        {
            float vidaActual = Mathf.Clamp(personajeAsociado.vida, 0, vidaMaxima);
            rellenoBarraVida.fillAmount = vidaActual / vidaMaxima;

            // Detecta si el personaje se murió y marca que ya no hay que seguir leyendo
            if (personajeAsociado.vida <= 0)
            {
                personajeMurio = true;
                rellenoBarraVida.fillAmount = 0f;
            }
        }
        else if (personajeMurio)
        {
            // Asegura que la barra quede vacía si el personaje fue destruido
            rellenoBarraVida.fillAmount = 0f;
        }
    }
}
