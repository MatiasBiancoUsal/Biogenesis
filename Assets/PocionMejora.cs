using UnityEngine;

public class PocionMejora : MonoBehaviour
{
    public int valor = 1; // unidades que suma esta poción (por defecto 1)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Intentamos obtener MutacionCazador directamente en el objeto que chocó
        MutacionCazador mutacion = collision.GetComponent<MutacionCazador>();

        // Si no está en el mismo GameObject, buscamos en padres (por si tu script está en un objeto padre)
        if (mutacion == null)
            mutacion = collision.GetComponentInParent<MutacionCazador>();

        if (mutacion != null)
        {
            mutacion.RecibirPocion();
            Destroy(gameObject); // la poción desaparece al ser recogida
            return;
        }

        // Si querés que también funcione recogiendo por tag "Criatura", opcional:
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
