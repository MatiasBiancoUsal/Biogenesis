using UnityEngine;
using System.Collections;

public class ParasitoHongo : MonoBehaviour
{
    public GameObject bolaPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    private Animator animator;
    private GameObject objetivo;

    void Start()
    {

        animator = GetComponent<Animator>();
        DetectarObjetivo();
        StartCoroutine(Comportamiento());

        //  Destruye el hongo a los 30 segundos
        Destroy(gameObject, 30f);
    }

    IEnumerator Comportamiento()
    {
        while (true)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(2f);

            animator.SetTrigger("Atacar");

            float tiempo = 5f;
            float t = 0f;
            while (t < tiempo)
            {
                LanzarBola();
                yield return new WaitForSeconds(intervaloDisparo);
                t += intervaloDisparo;
            }
        }
    }

    void LanzarBola()
    {
        GameObject bola = Instantiate(bolaPrefab, puntoDisparo.position, Quaternion.identity);
    Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();

    // Dirección aleatoria hacia la derecha
    Vector2 direccion = new Vector2(Random.Range(0.3f, 1f), Random.Range(-1f, 1f)).normalized;
    rb.velocity = direccion * 1.2f;

    // Tamaño aleatorio entre 0.15 y 0.31
    float escala = Random.Range(0.15f, 0.31f);
    bola.transform.localScale = new Vector3(escala, escala, 1f);
       
    }

    void DetectarObjetivo()
    {
        GameObject encontrado = GameObject.FindGameObjectWithTag("Criatura");
        if (encontrado != null)
        {
            objetivo = encontrado;
        }
    }
}
