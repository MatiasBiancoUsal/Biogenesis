using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservadorVidaAraña : MonoBehaviour
{
    public Personaje personaje; // Arrastrás al personaje araña acá en el inspector

    void Update()
    {
        if (personaje != null && EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.vidaAraña = personaje.vida;
            EstadoCriaturasGlobal.instancia.vidaMaxAraña = personaje.vidaMaxima;
        }
    }
}
