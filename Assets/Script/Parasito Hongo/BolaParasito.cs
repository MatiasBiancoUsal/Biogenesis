using UnityEngine;

public class BolaParásito : MonoBehaviour
{
    public int daño = 1;
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
            personaje.TomarDaño(daño);
            Destroy(gameObject);
        }
    }
}

