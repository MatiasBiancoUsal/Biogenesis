using UnityEngine;

public class ProyectilMutante : MonoBehaviour
{
    [Header("Configuración del proyectil")]
    public float velocidad = 6f;
    public int daño = 1; // cuánto daño hace el proyectil

    private Vector2 direccion;

    public void SetDirection(Vector2 dir)
    {
        direccion = dir.normalized;
    }

    void Update()
    {
        // mover proyectil
        transform.Translate(direccion * velocidad * Time.deltaTime);

        // destruir si se va demasiado lejos (opcional)
        if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 20)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("depredador"))
        {
            // busca el script DepredadorAnimTest
            DepredadorAnimTest enemigo = other.GetComponent<DepredadorAnimTest>();
            if (enemigo != null)
            {
                // aplicar daño según cantidad
                for (int i = 0; i < daño; i++)
                {
                    enemigo.RecibirDaño();
                }
            }

            Destroy(gameObject); // destruir proyectil al impactar
        }
    }
}
