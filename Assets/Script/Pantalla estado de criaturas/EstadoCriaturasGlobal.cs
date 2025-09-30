using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCriaturasGlobal : MonoBehaviour
{
    public static EstadoCriaturasGlobal instancia;

    [Header("Ara�a")]
    public int vidaAra�a = 100;
    public int vidaMaxAra�a = 100;

    [Header("Alima�a")]
    public int vidaAlima�a = 100;
    public int vidaMaxAlima�a = 100;

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
        PlayerPrefs.SetInt("Vida_Ara�a", vidaAra�a);
        PlayerPrefs.SetInt("Vida_Alima�a", vidaAlima�a);
        PlayerPrefs.SetInt("Vida_Mutante", vidaMutante);
        PlayerPrefs.SetInt("Vida_Cazador", vidaCazador);
        PlayerPrefs.Save();
    }

    public void CargarEstado()
    {
        vidaAra�a = PlayerPrefs.GetInt("Vida_Ara�a", vidaMaxAra�a);
        vidaAlima�a = PlayerPrefs.GetInt("Vida_Alima�a", vidaMaxAlima�a);
        vidaMutante = PlayerPrefs.GetInt("Vida_Mutante", vidaMaxMutante);
        vidaCazador = PlayerPrefs.GetInt("Vida_Cazador", vidaMaxCazador);
    }
}
