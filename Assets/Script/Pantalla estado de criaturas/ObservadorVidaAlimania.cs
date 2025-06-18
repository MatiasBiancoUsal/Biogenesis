using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservadorVidaAlimania : MonoBehaviour
{
    public Personaje personaje; // Arrastr�s al personaje ara�a ac� en el inspector

    void Update()
    {
        if (personaje != null && EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.vidaAlima�a = personaje.vida;
            EstadoCriaturasGlobal.instancia.vidaMaxAlima�a = personaje.vidaMaxima;
        }
    }
}
