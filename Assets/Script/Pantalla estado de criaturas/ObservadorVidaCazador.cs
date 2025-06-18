using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservadorVidaCazador : MonoBehaviour
{
    public Personaje personaje; // Arrastrás al personaje araña acá en el inspector

    void Update()
    {
        if (personaje != null && EstadoCriaturasGlobal.instancia != null)
        {
            EstadoCriaturasGlobal.instancia.vidaCazador = personaje.vida;
            EstadoCriaturasGlobal.instancia.vidaMaxCazador = personaje.vidaMaxima;
        }
    }
}
