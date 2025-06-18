using UnityEngine;

public class Personaje : MonoBehaviour
{
    public int vida = 100;
    public int vidaMaxima = 100;

    public void TomarDaño(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("Personaje recibió daño. Vida actual: " + vida);
        if (vida <= 0)
        {
            vida = 0;
            Debug.Log("Personaje ha muerto");
            Destroy(gameObject);
        }
    }

    public void RestaurarVida(int cantidad)
    {
        if (vida < vidaMaxima)
        {
            vida += cantidad;
            if (vida > vidaMaxima)
            {
                vida = vidaMaxima;
            }

            Debug.Log("Personaje se curó. Vida actual: " + vida);
        }
    }
}


