using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADNAlimaña : MonoBehaviour
{
    public GameObject prefabADN;
    public float intervalo = 5f; // cada cuánto tiempo genera ADN
    public Transform puntoGeneracion; // opcional: para definir un lugar específico

    public int maximoADN = 9;
    private int adnGenerados = 0;

    private float tiempoSiguiente = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (adnGenerados < maximoADN)
        {
            tiempoSiguiente -= Time.deltaTime;

            if (tiempoSiguiente <= 0f)
            {
                GenerarADN();
                tiempoSiguiente = intervalo;
            }
        }
    }

    void GenerarADN()
    {
        // Definí los límites del área donde querés que aparezca el ADN
        float x = Random.Range(-5f, 5f); // Cambiá estos valores según tu escena
        float y = Random.Range(-3f, 3f);
        Vector3 posicion = new Vector3(x, y, 0f);

        Instantiate(prefabADN, posicion, Quaternion.identity);

        adnGenerados++;
        Debug.Log($"ADN generados: {adnGenerados}/{maximoADN}");
    }
}
