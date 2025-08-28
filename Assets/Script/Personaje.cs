using UnityEngine;

public class Personaje : MonoBehaviour
{
    public int vida = 100;
    public int vidaMaxima = 100;

    // Script lucy
    private Animator animator;
    private bool estaMuerto = false;
    public enum TipoMutacion { Mutacion1, Mutacion2, Mutacion3 }
    public TipoMutacion mutacionActual;

    void Start()
    {
        animator = GetComponent<Animator>(); // buscamos el Animator en el personaje
    }
//
    public void TomarDaño(int cantidad)
    {
        if (estaMuerto) return; // si ya murió, no recibe más daño

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

    private void Morir()
    {
        estaMuerto = true;
        Debug.Log("Personaje ha muerto");
        switch (mutacionActual)
        {
            case TipoMutacion.Mutacion1:
                animator.SetTrigger("muerte1");
                break;
            case TipoMutacion.Mutacion2:
                animator.SetTrigger("muerte2");
                break;
            case TipoMutacion.Mutacion3:
                animator.SetTrigger("muerte3");
                break;
        }
    }
}


