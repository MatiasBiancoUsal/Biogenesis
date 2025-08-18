using UnityEngine;

public class ProyectilTelaraña : MonoBehaviour
{
    public float velocidad = 7f;
    public int daño = 1;
    public Vector2 direccion;
    public float tiempoVida = 4f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("depredador"))
        {
            DepredadorAnimTest dep = collision.GetComponent<DepredadorAnimTest>();
            if (dep != null)
            {
                dep.SendMessage("RecibirDaño", SendMessageOptions.DontRequireReceiver);
            }

            Destroy(gameObject);
        }
    }
}