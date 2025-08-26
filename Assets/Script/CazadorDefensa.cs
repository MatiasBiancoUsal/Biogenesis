using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public float intervaloAtaque = 1.5f;
    public float rangoDeteccion = 2f; // rango corto, cuerpo a cuerpo
    public int daño = 10;

    private bool atacando = false;

    void Update()
    {
        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !atacando)
        {
            Debug.Log($"[Cazador] Enemigo detectado: {enemigo.name}, comenzando ataque.");
            StartCoroutine(AtacarRoutine(enemigo.transform));
        }
    }

    GameObject DetectarEnemigo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito"))
            {
                Debug.Log($"[Cazador] Detectado enemigo en rango: {hit.name}");
                return hit.gameObject;
            }
        }
        return null;
    }

    IEnumerator AtacarRoutine(Transform objetivo)
    {
        atacando = true;
        while (objetivo != null)
        {
            Atacar(objetivo);
            yield return new WaitForSeconds(intervaloAtaque);
        }
        atacando = false;
        Debug.Log("[Cazador] Dejó de atacar (objetivo perdido).");
    }

    void Atacar(Transform objetivo)
    {
        // Intentamos obtener el script de vida del enemigo
        //var vida = objetivo.GetComponent<EnemigoVida>();
       // if (vida != null)
        //{
          //  vida.RecibirDaño(daño);
           // Debug.Log($"[Cazador] Infligió {daño} de daño a {objetivo.name}");
        //}

        // Acá podrías activar animación de ataque
        // GetComponent<Animator>().SetTrigger("Atacar");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
