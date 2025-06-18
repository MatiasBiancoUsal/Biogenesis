using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservadorVidaMutante : MonoBehaviour
{
    public Personaje personaje; // Arrastr�s al personaje ara�a ac� en el inspector

    void Update()
    {
        if (personaje != null && EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.vidaMutante = personaje.vida;
            EstadoCriaturasGlobal.instancia.vidaMaxMutante = personaje.vidaMaxima;
        }
    }
}
