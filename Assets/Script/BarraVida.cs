using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    public Personaje personajeAsociado; // Lo mantenemos para saber la vida MAX
    public string creatureID;

    private int vidaMaxima;
    private int vidaActual;

    void Start()
    {
        // Obtenemos la vida máxima
        switch (creatureID)
        {
            case "Araña": vidaMaxima = EstadoCriaturasGlobal.instancia.vidaMaxAraña; break;
            case "Alimaña": vidaMaxima = EstadoCriaturasGlobal.instancia.vidaMaxAlimaña; break;
            case "Mutante": vidaMaxima = EstadoCriaturasGlobal.instancia.vidaMaxMutante; break;
            case "Cazador": vidaMaxima = EstadoCriaturasGlobal.instancia.vidaMaxCazador; break;
        }

        // Si tu script 'Personaje' necesita saber su vida máxima, asígnala aquí
        if (personajeAsociado != null)
        {
            // personajeAsociado.vidaMaxima = vidaMaxima; // (Si existe esa variable)
        }
    }

    void Update()
    {
        if (EstadoCriaturasGlobal.instancia == null) return;

        // --- ¡CAMBIO CLAVE! ---
        // 1. LEEMOS la vida actual desde el Singleton
        switch (creatureID)
        {
            case "Araña": vidaActual = EstadoCriaturasGlobal.instancia.vidaAraña; break;
            case "Alimaña": vidaActual = EstadoCriaturasGlobal.instancia.vidaAlimaña; break;
            case "Mutante": vidaActual = EstadoCriaturasGlobal.instancia.vidaMutante; break;
            case "Cazador": vidaActual = EstadoCriaturasGlobal.instancia.vidaCazador; break;
        }

        // 2. Actualizamos la barra local
        rellenoBarraVida.fillAmount = (float)vidaActual / vidaMaxima;

        // 3. (Opcional) Sincronizamos el script del personaje local
        if (personajeAsociado != null)
        {
            personajeAsociado.vida = vidaActual;
        }

        // 4. ¡YA NO GUARDAMOS NADA AQUÍ!
        // PlayerPrefs.SetInt(...) -> ELIMINADO
        // EstadoCriaturasGlobal.instancia.GuardarEstado() -> ELIMINADO
    }
}