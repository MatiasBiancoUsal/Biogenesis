using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectilcazador : MonoBehaviour
{

    public float velocidad = 5f;
    public Vector2 direccion = Vector2.right;
    public int daño = 1; // daño que hace al personaje
    public float tiempoVida = 5f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f); // destruir después de 5 segundos
        // Destruir automáticamente después de cierto tiempo
        Destroy(gameObject, tiempoVida);

    }

    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion.normalized * velocidad * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si toca al Depredador, destruir
        if (collision.CompareTag("Depredador"))
        {
            Destroy(gameObject);
        }

        // Si toca a un objeto con el script "Personaje", hacerle daño y destruir
        Personaje personaje = collision.GetComponent<Personaje>();
        if (personaje != null)
        {
            personaje.TomarDaño(daño);
            Destroy(gameObject);
        }
    }
}
