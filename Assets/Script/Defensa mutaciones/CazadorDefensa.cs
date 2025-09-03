using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public float intervaloAtaque = 1.5f;
    public float rangoDeteccion = 2f; // Rango cuerpo a cuerpo
    public int da�o = 10;

    private bool atacando = false;
    private Animator anim;

    
    public enum EstadoMutacion { Normal, Primera, Final }
    public EstadoMutacion estado = EstadoMutacion.Normal;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Iniciar ciclo de animaciones
        StartCoroutine(CicloAnimaciones());
    }

    void Update()
    {
        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !atacando)
        {
            Debug.Log($"[Cazador] Enemigo detectado: {enemigo.name}, comenzando ataque.");
            StartCoroutine(AtacarRoutine(enemigo.transform));
        }
    }

    // --------------------
    // Detecci�n de enemigos
    // --------------------
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

    // --------------------
    // Rutina de ataque
    // --------------------
    IEnumerator AtacarRoutine(Transform objetivo)
    {
        atacando = true;
        while (objetivo != null)
        {
            Atacar(objetivo);
            yield return new WaitForSeconds(intervaloAtaque);
        }
        atacando = false;
        Debug.Log("[Cazador] Dej� de atacar (objetivo perdido).");
    }

    // --------------------
    // M�todo de ataque
    // --------------------
    void Atacar(Transform objetivo)
    {
        // Animaci�n seg�n mutaci�n
        switch (estado)
        {
            case EstadoMutacion.Normal: anim.SetTrigger("Atacar1"); break;
            case EstadoMutacion.Primera: anim.SetTrigger("Atacar2"); break;
            case EstadoMutacion.Final: anim.SetTrigger("Atacar3"); break;
        }

        // Aplicar da�o al objetivo si tiene script de vida
        var criatura = objetivo.GetComponent<DepredadorAnimTest>();
        if (criatura != null)
        {
            criatura.RecibirDa�o();
            Debug.Log($"[Cazador] Infligi� {da�o} de da�o a {objetivo.name}");
        }
    }

    // --------------------
    // Ciclo de animaciones b�sicas (Idle/Disparar)
    // --------------------
    IEnumerator CicloAnimaciones()
    {
        while (true)
        {
            anim.SetTrigger("idle");
            yield return new WaitForSeconds(2f);
        }
    }

    // --------------------
    // Visualizar rango de detecci�n en el editor
    // --------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
