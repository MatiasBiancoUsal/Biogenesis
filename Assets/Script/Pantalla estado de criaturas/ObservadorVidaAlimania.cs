using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservadorVidaAlimania : MonoBehaviour
{
    public Personaje personaje; // Arrastrás al personaje araña acá en el inspector

    void Update()
    {
        if (personaje != null && EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.vidaAlimaña = personaje.vida;
            EstadoCriaturasGlobal.instancia.vidaMaxAlimaña = personaje.vidaMaxima;
        }
    }
}
