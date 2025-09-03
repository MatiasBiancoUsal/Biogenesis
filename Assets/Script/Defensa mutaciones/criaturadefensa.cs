using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class criaturadefensa : MonoBehaviour
{

    [Header("Ataque")]
    public float intervaloAtaque = 1.5f;
    public float rangoDeteccion = 2f; // Rango cuerpo a cuerpo
    public int da�o = 10;

    private bool atacando = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GameObject criatura = DetectarCriatura();
        if (criatura != null && !atacando)
        {
            StartCoroutine(AtacarRoutine(criatura.transform));
        }
    }

    // --------------------
    // Detecci�n de la criatura experimento
    // --------------------
    GameObject DetectarCriatura()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador"))
            {
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
    }

    // --------------------
    // Aplicar da�o
    // --------------------
    void Atacar(Transform objetivo)
    {
        var criatura = objetivo.GetComponent<DepredadorAnimTest>();
        if (criatura != null)
        {
            criatura.RecibirDa�o();
            Debug.Log($"[criatura] Infligi� {da�o} de da�o a {objetivo.name}");
            anim.SetTrigger("ataque1");
        }
    }

    // --------------------
    // Visualizar rango de detecci�n
    // --------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}

