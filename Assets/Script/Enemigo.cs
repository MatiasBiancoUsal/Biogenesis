using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int daņo = 30;
    public float intervaloDaņo = 1f; 
    private float tiempoSiguienteDaņo = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Personaje personaje = other.GetComponent<Personaje>();
        if (personaje != null && Time.time >= tiempoSiguienteDaņo)
        {
            personaje.TomarDaņo(daņo);
            tiempoSiguienteDaņo = Time.time + intervaloDaņo;
        }
    }
}
