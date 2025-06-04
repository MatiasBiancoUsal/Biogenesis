using UnityEngine;

public class VidaPickup : MonoBehaviour
{
    public int cantidadVida = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Personaje personaje = other.GetComponent<Personaje>();
        if (personaje != null)
        {
            personaje.RestaurarVida(cantidadVida);
            Destroy(gameObject); 
        }
    }
}
