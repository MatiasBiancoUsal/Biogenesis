using UnityEngine;

public class PocionMejora : MonoBehaviour
{
    public int valor = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Buscamos cualquier script que implemente IMutable (puede estar en el objeto, hijo o padre)
        var mutable = collision.GetComponent<IMutable>() ??
                      collision.GetComponentInChildren<IMutable>() ??
                      collision.GetComponentInParent<IMutable>();

        if (mutable != null)
        {
            mutable.RecibirPocion();
            Destroy(gameObject); // destruir la poción
        }
    }
}
