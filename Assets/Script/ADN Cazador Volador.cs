using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADNCazadorVolador : MonoBehaviour
{
    public GameObject prefabADN;
    public float intervalo = 5f; // cada cu�nto tiempo genera ADN
    public Transform puntoGeneracion; // opcional: para definir un lugar espec�fico

    private float tiempoSiguiente = 120f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tiempoSiguiente -= Time.deltaTime;

        if (tiempoSiguiente <= 0f)
        {
            GenerarADN();
            tiempoSiguiente = intervalo;
        }
    }

    void GenerarADN()
    {
        // Defin� los l�mites del �rea donde quer�s que aparezca el ADN
        float x = Random.Range(-5f, 5f); // Cambi� estos valores seg�n tu escena
        float y = Random.Range(-3f, 3f);
        Vector3 posicion = new Vector3(x, y, 0f);

        Instantiate(prefabADN, posicion, Quaternion.identity);
    }
}
