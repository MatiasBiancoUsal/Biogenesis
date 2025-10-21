using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCriaturasGlobal : MonoBehaviour
{
    public static EstadoCriaturasGlobal instancia;

    [Header("Tasas de Hambre (ej. puntos por segundo)")]
    public float tasaHambreAra�a = 0.5f; // Medio punto por segundo
    public float tasaHambreAlima�a = 0.5f;
    public float tasaHambreMutante = 0.5f;
    public float tasaHambreCazador = 0.5f;

    private float acumuladorAra�a = 0f;
    private float acumuladorAlima�a = 0f;
    private float acumuladorMutante = 0f;
    private float acumuladorCazador = 0f;

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

    void Update()
    {
        // --- L�gica de Hambre Pasiva ---
        // Usamos acumuladores para perder 'int' de vida basado en el tiempo

        acumuladorAra�a += tasaHambreAra�a * Time.deltaTime;
        if (acumuladorAra�a >= 1.0f)
        {
            vidaAra�a -= 1;
            acumuladorAra�a -= 1.0f; // Restamos 1, pero mantenemos el decimal restante
        }

        acumuladorAlima�a += tasaHambreAlima�a * Time.deltaTime;
        if (acumuladorAlima�a >= 1.0f)
        {
            vidaAlima�a -= 1;
            acumuladorAlima�a -= 1.0f;
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

        // --- Asegurar L�mites (Clamping) ---
        // Nos aseguramos de que la vida nunca baje de 0
        vidaAra�a = Mathf.Clamp(vidaAra�a, 0, vidaMaxAra�a);
        vidaAlima�a = Mathf.Clamp(vidaAlima�a, 0, vidaMaxAlima�a);
        vidaMutante = Mathf.Clamp(vidaMutante, 0, vidaMaxMutante);
        vidaCazador = Mathf.Clamp(vidaCazador, 0, vidaMaxCazador);
    }

    public void ModificarVida(string creatureID, int cantidad)
    {
        switch (creatureID)
        {
            case "Ara�a": vidaAra�a += cantidad; break;
            case "Alima�a": vidaAlima�a += cantidad; break;
            case "Mutante": vidaMutante += cantidad; break;
            case "Cazador": vidaCazador += cantidad; break;
        }
        GuardarEstado();
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
