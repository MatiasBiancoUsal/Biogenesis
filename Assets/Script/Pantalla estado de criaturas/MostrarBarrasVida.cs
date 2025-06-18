using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarBarrasVida : MonoBehaviour
{
    public Image barraAraña;
    public Image barraAlimaña;
    public Image barraMutante;
    public Image barraCazador;

    void Update()
    {
        if (EstadoCriaturasGlobal.instancia == null) return;

        barraAraña.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAraña / EstadoCriaturasGlobal.instancia.vidaMaxAraña);
        barraAlimaña.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAlimaña / EstadoCriaturasGlobal.instancia.vidaMaxAlimaña);
        barraMutante.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaMutante / EstadoCriaturasGlobal.instancia.vidaMaxMutante);
        barraCazador.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaCazador / EstadoCriaturasGlobal.instancia.vidaMaxCazador);
    }
}
