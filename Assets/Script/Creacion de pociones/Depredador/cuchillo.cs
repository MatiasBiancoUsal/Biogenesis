using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuchillo : MonoBehaviour
{
    
    public int daño = 10;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta si choca con un personaje
        Personaje personaje = collision.GetComponent<Personaje>();
        if (personaje != null)
        {
            personaje.TomarDaño(daño, "depredador"); // Llama a la función que reduce la vida
            Destroy(gameObject); // Destruye el cuchillo
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
