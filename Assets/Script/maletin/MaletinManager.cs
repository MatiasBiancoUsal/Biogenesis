using System.Collections.Generic;
using UnityEngine;

public class MaletinManager : MonoBehaviour
{
    public static MaletinManager instancia;

    public GameObject pocionVida;
    public GameObject pocionMejora;

    public bool tienePocionVida()
    {
        return pocionVida != null;
    }

    public bool tienePocionMejora()
    {
        return pocionMejora != null;
    }

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        // Solo la primera vez se inicializa vacío
        if (pocionVida != null || pocionMejora != null)
        {
            pocionVida = null;
            pocionMejora = null;
        }
    }

    public void GuardarPocion(GameObject prefabPocion)
    {
        if (prefabPocion.CompareTag("PocionVida"))
        {
            pocionVida = prefabPocion;
        }
        else if (prefabPocion.CompareTag("PocionMejora"))
        {
            pocionMejora = prefabPocion;
        }
    }

    public List<GameObject> ObtenerPociones()
    {
        var lista = new List<GameObject>();
        if (pocionVida != null) lista.Add(pocionVida);
        if (pocionMejora != null) lista.Add(pocionMejora);
        return lista;
    }
}