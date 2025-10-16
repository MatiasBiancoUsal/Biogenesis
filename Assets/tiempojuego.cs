using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiempojuego : MonoBehaviour
{
    public static tiempojuego instance;

    public float tiempo;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        tiempo += Time.deltaTime;
    }
}
