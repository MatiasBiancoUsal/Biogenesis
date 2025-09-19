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

        // Desactiva el bot�n al inicio, para que no pueda ser presionado
        if (boton != null)
        {
            boton.interactable = false;
        }
    }

    void Start()
    {
        // Llama a la funci�n para actualizar el estado del bot�n al cargar la escena
        ActualizarEstadoBoton();
    }

    // Esta funci�n se puede llamar desde otros scripts para actualizar el estado del bot�n
    public void ActualizarEstadoBoton()
    {
        // Verifica si la instancia del manager existe y si la criatura ya fue creada
        if (CriaturaCreada.Instance != null && boton != null)
        {
            // El bot�n solo ser� interactuable si la criatura fue creada
            boton.interactable = CriaturaCreada.Instance.criaturaCreada;
        }
    }
}
