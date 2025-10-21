using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCriaturasGlobal : MonoBehaviour
{
    public static EstadoCriaturasGlobal instancia;

    [Header("Tasas de Hambre (ej. puntos por segundo)")]
    public float tasaHambreAraña = 0.5f; // Medio punto por segundo
    public float tasaHambreAlimaña = 0.5f;
    public float tasaHambreMutante = 0.5f;
    public float tasaHambreCazador = 0.5f;

    private float acumuladorAraña = 0f;
    private float acumuladorAlimaña = 0f;
    private float acumuladorMutante = 0f;
    private float acumuladorCazador = 0f;

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

    void Update()
    {
        // --- Lógica de Hambre Pasiva ---
        // Usamos acumuladores para perder 'int' de vida basado en el tiempo

        acumuladorAraña += tasaHambreAraña * Time.deltaTime;
        if (acumuladorAraña >= 1.0f)
        {
            vidaAraña -= 1;
            acumuladorAraña -= 1.0f; // Restamos 1, pero mantenemos el decimal restante
        }

        acumuladorAlimaña += tasaHambreAlimaña * Time.deltaTime;
        if (acumuladorAlimaña >= 1.0f)
        {
            vidaAlimaña -= 1;
            acumuladorAlimaña -= 1.0f;
        }

        acumuladorMutante += tasaHambreMutante * Time.deltaTime;
        if (acumuladorMutante >= 1.0f)
        {
            vidaMutante -= 1;
            acumuladorMutante -= 1.0f;
        }

        acumuladorCazador += tasaHambreCazador * Time.deltaTime;
        if (acumuladorCazador >= 1.0f)
        {
            vidaCazador -= 1;
            acumuladorCazador -= 1.0f;
        }

        // --- Asegurar Límites (Clamping) ---
        // Nos aseguramos de que la vida nunca baje de 0
        vidaAraña = Mathf.Clamp(vidaAraña, 0, vidaMaxAraña);
        vidaAlimaña = Mathf.Clamp(vidaAlimaña, 0, vidaMaxAlimaña);
        vidaMutante = Mathf.Clamp(vidaMutante, 0, vidaMaxMutante);
        vidaCazador = Mathf.Clamp(vidaCazador, 0, vidaMaxCazador);
    }

    public void ModificarVida(string creatureID, int cantidad)
    {
        switch (creatureID)
        {
            case "Araña": vidaAraña += cantidad; break;
            case "Alimaña": vidaAlimaña += cantidad; break;
            case "Mutante": vidaMutante += cantidad; break;
            case "Cazador": vidaCazador += cantidad; break;
        }
        GuardarEstado();
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

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            GuardarEstado();
        }
    }

    void OnApplicationQuit()
    {
        GuardarEstado();
    }
}
