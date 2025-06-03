using UnityEngine;

public class EnemigoDanio : MonoBehaviour
{
    public int danio = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rata"))
        {
            VidaPersonaje vida = other.GetComponent<VidaPersonaje>();
            if (vida != null)
            {
                vida.RecibirDanio(danio);
            }
        }
    }
}
