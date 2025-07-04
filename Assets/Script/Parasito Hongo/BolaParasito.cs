using UnityEngine;

public class BolaPar�sito : MonoBehaviour
{
    public int da�o = 1;
    public float tiempoVida = 10f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // opcional: movimiento flotante
    }

    void OnMouseDown()
    {
        Destroy(gameObject); // si la criatura la toca
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Personaje personaje = other.GetComponent<Personaje>();
        if (personaje != null)
        {
            personaje.TomarDa�o(da�o);
            Destroy(gameObject);
        }
    }
}

