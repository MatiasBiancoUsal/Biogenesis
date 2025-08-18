using System.Collections;
using UnityEngine;

public class ArañaDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    public float rangoDeteccion = 6f;

    private bool disparando = false;

    void Update()
    {
        GameObject enemigo = DetectarDepredador();
        if (enemigo != null && !disparando)
        {
            StartCoroutine(DispararRoutine(enemigo.transform));
        }
    }

    GameObject DetectarDepredador()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador")) // 👈 importante: el tag debe estar en minúsculas como lo tenés
            {
                return hit.gameObject;
            }
        }
        return null;
    }

    IEnumerator DispararRoutine(Transform objetivo)
    {
        disparando = true;
        while (objetivo != null)
        {
            Disparar(objetivo);
            yield return new WaitForSeconds(intervaloDisparo);
        }
        disparando = false;
    }

    void Disparar(Transform objetivo)
    {
        if (proyectilPrefab != null && puntoDisparo != null)
        {
            GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
            proyectil.tag = "ProyectilEnemigo"; // 👈 para que el depredador lo detecte

            Vector2 direccion = (objetivo.position - puntoDisparo.position).normalized;
            proyectil.GetComponent<ProyectilTelaraña>().direccion = direccion;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}


