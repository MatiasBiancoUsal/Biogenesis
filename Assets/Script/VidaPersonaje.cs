using UnityEngine;

public class VidaPersonaje : MonoBehaviour
{
    public int vidaMaxima = 100;
    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    
    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log("Recibió daño. Vida actual: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
       
        Destroy(gameObject); //esto por si quiero q desaparezca o haga animación
    }
}

