using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarBarrasVida : MonoBehaviour
{
    public Image barraAra�a;
    public Image barraAlima�a;
    public Image barraMutante;
    public Image barraCazador;

    void Update()
    {
        if (EstadoCriaturasGlobal.instancia == null) return;

        barraAra�a.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAra�a / EstadoCriaturasGlobal.instancia.vidaMaxAra�a);
        barraAlima�a.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaAlima�a / EstadoCriaturasGlobal.instancia.vidaMaxAlima�a);
        barraMutante.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaMutante / EstadoCriaturasGlobal.instancia.vidaMaxMutante);
        barraCazador.fillAmount = Mathf.Clamp01((float)EstadoCriaturasGlobal.instancia.vidaCazador / EstadoCriaturasGlobal.instancia.vidaMaxCazador);
    }
}
