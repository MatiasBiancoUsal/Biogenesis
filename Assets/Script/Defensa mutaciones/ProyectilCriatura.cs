using UnityEngine;

public class ProyectilCriatura : MonoBehaviour
{
    public int damage = 1;       // da�o que hace al enemigo
    public float speed = 6f;     // velocidad del proyectil
    public float lifetime = 4f;  // se destruye solo despu�s de este tiempo

    private Vector2 direction;   // direcci�n hacia donde va

    // Llamar al instanciar el proyectil
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Movimiento manual usando Kinematic Rigidbody
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("depredador"))  // tag del enemigo
        {
            // Llama al m�todo de da�o del enemigo
            var dep = collision.GetComponent<DepredadorAnimTest>();
            if (dep != null)
            {
                dep.RecibirDa�o();
            }

            Destroy(gameObject);  // se destruye al impactar
        }
    }
}
