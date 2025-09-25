using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilMutante : MonoBehaviour
{
    public float velocidad = 6f;
    private Vector2 direccion;

    public void SetDirection(Vector2 dir)
    {
        direccion = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
        Debug.Log("Moviendo proyectil en dirección: " + direccion);

        // destruir si se va demasiado lejos (opcional)
        if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 20)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("depredador"))
        {
            // acá podés hacer daño al enemigo si tenés un script de vida
            Destroy(gameObject);
        }
    }
}
