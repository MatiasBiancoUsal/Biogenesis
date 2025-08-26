using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlimañaDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    public float rangoDeteccion = 6f;

    private bool disparando = false;

    void Update()
    {
        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !disparando)
        {
            StartCoroutine(DispararRoutine(enemigo.transform));
        }
    }

    GameObject DetectarEnemigo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        DerivadoAutoMover movimiento = GetComponent<DerivadoAutoMover>();
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito")) // ✅ Ahora detecta los dos
            {

                movimiento.quieto = true;
                return hit.gameObject;
            }
        }
        movimiento.quieto = false;
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
            proyectil.tag = "ProyectilEnemigo";

            Vector2 direccion = (objetivo.position - puntoDisparo.position).normalized;
            proyectil.GetComponent<ProyectilExplosivo>().direccion = direccion;
            var scriptProyectil = proyectil.GetComponent<ProyectilExplosivo>();
            scriptProyectil.direccion = direccion;

            // Buscamos si hay MutacionAlimaña en este mismo GameObject
            MutacionAlimaña mutacion = GetComponent<MutacionAlimaña>();
            if (mutacion != null)
            {
                scriptProyectil.daño = Mathf.RoundToInt(scriptProyectil.daño * mutacion.ObtenerMultiplicadorDaño());
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}

