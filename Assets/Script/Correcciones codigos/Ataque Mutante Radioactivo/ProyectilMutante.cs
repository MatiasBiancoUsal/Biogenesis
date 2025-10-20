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
        if (other.CompareTag("depredador") || other.CompareTag("Parasito"))
        {
            // Intenta encontrar un script de daño en cualquiera de los dos tipos
            DepredadorAnimTest depredador = other.GetComponent<DepredadorAnimTest>();
            ParasitoHongo hongo = other.GetComponent<ParasitoHongo>(); // o el nombre que uses para tu script

            if (depredador != null)
            {
                for (int i = 0; i < daño; i++)
                    depredador.RecibirDaño();
            }
            else if (hongo != null)
            {
                for (int i = 0; i < daño; i++)
                    hongo.RecibirDaño();
            }
            else
            {
                Debug.LogWarning("No se encontró script de daño en el objetivo con tag: " + other.tag);
            }

            Destroy(gameObject);
        }
    }
}
