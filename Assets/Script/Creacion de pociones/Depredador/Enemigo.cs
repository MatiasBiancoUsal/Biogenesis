using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int daño = 30;
    public float intervaloDaño = 1f; 
    private float tiempoSiguienteDaño = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Personaje personaje = other.GetComponent<Personaje>();
        if (personaje != null && Time.time >= tiempoSiguienteDaño)
        {
            //personaje.TomarDaño(daño);
            //tiempoSiguienteDaño = Time.time + intervaloDaño;
        }
    }
}
