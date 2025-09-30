using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCriaturasGlobal : MonoBehaviour
{
    public static EstadoCriaturasGlobal instancia;

    [Header("Araña")]
    public int vidaAraña = 100;
    public int vidaMaxAraña = 100;

    [Header("Alimaña")]
    public int vidaAlimaña = 100;
    public int vidaMaxAlimaña = 100;

    [Header("Mutante")]
    public int vidaMutante = 100;
    public int vidaMaxMutante = 100;

    [Header("Cazador")]
    public int vidaCazador = 100;
    public int vidaMaxCazador = 100;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
            CargarEstado();
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    public void GuardarEstado()
    {
        PlayerPrefs.SetInt("Vida_Araña", vidaAraña);
        PlayerPrefs.SetInt("Vida_Alimaña", vidaAlimaña);
        PlayerPrefs.SetInt("Vida_Mutante", vidaMutante);
        PlayerPrefs.SetInt("Vida_Cazador", vidaCazador);
        PlayerPrefs.Save();
    }

    public void CargarEstado()
    {
        vidaAraña = PlayerPrefs.GetInt("Vida_Araña", vidaMaxAraña);
        vidaAlimaña = PlayerPrefs.GetInt("Vida_Alimaña", vidaMaxAlimaña);
        vidaMutante = PlayerPrefs.GetInt("Vida_Mutante", vidaMaxMutante);
        vidaCazador = PlayerPrefs.GetInt("Vida_Cazador", vidaMaxCazador);
    }
}
