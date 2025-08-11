using UnityEngine;

public class PocionMejora : MonoBehaviour
{
    public int valor = 1; // unidades que suma esta poci�n (por defecto 1)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Intentamos obtener MutacionCazador directamente en el objeto que choc�
        MutacionCazador mutacion = collision.GetComponent<MutacionCazador>();

        // Si no est� en el mismo GameObject, buscamos en padres (por si tu script est� en un objeto padre)
        if (mutacion == null)
            mutacion = collision.GetComponentInParent<MutacionCazador>();

        if (mutacion != null)
        {
            mutacion.RecibirPocion();
            Destroy(gameObject); // la poci�n desaparece al ser recogida
            return;
        }

        // Si quer�s que tambi�n funcione recogiendo por tag "Criatura", opcional:
        if (collision.CompareTag("Criatura"))
        {
            // Intentamos encontrar MutacionCazador en la criatura por si no lo agarramos arriba
            GameObject go = collision.gameObject;
            MutacionCazador mc = go.GetComponent<MutacionCazador>() ?? go.GetComponentInChildren<MutacionCazador>() ?? go.GetComponentInParent<MutacionCazador>();
            if (mc != null)
            {
                mc.RecibirPocion();
                Destroy(gameObject);
            }
        }
    }
}
