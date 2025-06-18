using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservadorVidaArania : MonoBehaviour
{
    public Personaje personaje; // Arrastr�s al personaje ara�a ac� en el inspector

    void Update()
    {
        if (personaje != null && EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.vidaAra�a = personaje.vida;
            EstadoCriaturasGlobal.instancia.vidaMaxAra�a = personaje.vidaMaxima;
        }
    }
}
