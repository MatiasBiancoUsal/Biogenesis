using System.Collections.Generic;
using UnityEngine;

public class MaletinManager : MonoBehaviour
{
    public static MaletinManager instancia;
    public List<GameObject> pociones = new List<GameObject>();

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AgregarPocion(GameObject pocion)
    {
        pociones.Add(pocion);
    }
}