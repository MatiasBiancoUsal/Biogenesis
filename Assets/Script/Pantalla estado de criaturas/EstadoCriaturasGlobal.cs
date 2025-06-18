using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCriaturasGlobal : MonoBehaviour
{
    public static EstadoCriaturasGlobal instancia;

    public int vidaAraña = 100;
    public int vidaMaxAraña = 100;

    public int vidaAlimaña = 100;
    public int vidaMaxAlimaña = 100;

    public int vidaMutante = 100;
    public int vidaMaxMutante = 100;

    public int vidaCazador = 100;
    public int vidaMaxCazador = 100;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
}
