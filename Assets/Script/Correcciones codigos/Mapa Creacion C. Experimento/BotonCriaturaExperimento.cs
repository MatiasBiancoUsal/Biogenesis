using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonCriaturaExperimento : MonoBehaviour
{
    private Button boton;

    void Awake()
    {
        // Obtiene el componente Button del mismo objeto.
        boton = GetComponent<Button>();

        // Desactiva el botón al inicio, para que no pueda ser presionado
        if (boton != null)
        {
            boton.interactable = false;
        }
    }

    void Start()
    {
        // Llama a la función para actualizar el estado del botón al cargar la escena
        ActualizarEstadoBoton();
    }

    // Esta función se puede llamar desde otros scripts para actualizar el estado del botón
    public void ActualizarEstadoBoton()
    {
        // Verifica si la instancia del manager existe y si la criatura ya fue creada
        if (CriaturaCreada.Instance != null && boton != null)
        {
            // El botón solo será interactuable si la criatura fue creada
            boton.interactable = CriaturaCreada.Instance.criaturaCreada;
        }
    }
}
