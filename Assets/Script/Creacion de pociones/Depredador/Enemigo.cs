using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int da�o = 30;
    public float intervaloDa�o = 1f; 
    private float tiempoSiguienteDa�o = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Personaje personaje = other.GetComponent<Personaje>();
        if (personaje != null && Time.time >= tiempoSiguienteDa�o)
        {
            //personaje.TomarDa�o(da�o);
            //tiempoSiguienteDa�o = Time.time + intervaloDa�o;
        }
    }
}
