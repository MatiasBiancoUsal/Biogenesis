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
    private DerivadoAutoMover movimiento;

    // Guardamos la escala/posición inicial para no asumir signos
    float initialScaleX;
    float leftScaleX;   // valor de scale.x que corresponda a "mirar izquierda"
    float rightScaleX;  // contrario a leftScaleX
    float initialFirePointLocalX;

    [Header("Orientación")]
    [Tooltip("Marca true si el sprite en el editor APUNTA A LA IZQUIERDA por defecto (según dijiste). Si al revés, marca false).")]
    public bool spriteFacesLeftByDefault = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip disparoClip;

    void Start()
    {
        movimiento = GetComponent<DerivadoAutoMover>();

        initialScaleX = transform.localScale.x;
        // Calculamos qué valor de localScale.x corresponde a 'izquierda' y 'derecha'
        leftScaleX = spriteFacesLeftByDefault ? Mathf.Abs(initialScaleX) : -Mathf.Abs(initialScaleX);
        rightScaleX = -leftScaleX;

        if (puntoDisparo != null)
            initialFirePointLocalX = puntoDisparo.localPosition.x;
    }

    void Update()
    {
        GameObject enemigo = DetectarEnemigo();
        if (enemigo != null && !disparando)
            StartCoroutine(DispararRoutine(enemigo.transform));
    }

    GameObject DetectarEnemigo()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rangoDeteccion);

        foreach (var hit in hits)
        {
            if (hit == null) continue;
            if (hit.CompareTag("depredador") || hit.CompareTag("Parasito"))
            {
                if (movimiento != null) movimiento.quieto = true;

                // FORZAR mirar siempre a la izquierda (convención segura)
                ForceFaceLeft();

                return hit.gameObject;
            }
        }

        // Si no hay enemigo, volver a permitir movimiento
        if (movimiento != null) movimiento.quieto = false;
        return null;
    }

    IEnumerator DispararRoutine(Transform objetivo)
    {
        disparando = true;

        while (objetivo != null)
        {
            // aseguramos orientación y punto de disparo
            ForceFaceLeft();

            // NO tocamos el Animator aquí: DerivadoAutoMover ya setea "atacar" según "quieto"
            Disparar(objetivo);
            yield return new WaitForSeconds(intervaloDisparo);
        }

        disparando = false;
    }

    void ForceFaceLeft()
    {
        Vector3 s = transform.localScale;
        s.x = leftScaleX;
        transform.localScale = s;

        if (puntoDisparo != null)
            puntoDisparo.localPosition = new Vector3(initialFirePointLocalX, puntoDisparo.localPosition.y, puntoDisparo.localPosition.z);
    }

    // (por si después necesitás forzar derecha)
    void ForceFaceRight()
    {
        Vector3 s = transform.localScale;
        s.x = rightScaleX;
        transform.localScale = s;

        if (puntoDisparo != null)
            puntoDisparo.localPosition = new Vector3(-initialFirePointLocalX, puntoDisparo.localPosition.y, puntoDisparo.localPosition.z);
    }

    void Disparar(Transform objetivo)
    {
        if (proyectilPrefab == null || puntoDisparo == null || objetivo == null) return;

        GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);
        proyectil.tag = "ProyectilEnemigo";

        Vector2 direccion = (objetivo.position - puntoDisparo.position).normalized;
        ProyectilTelaraña p = proyectil.GetComponent<ProyectilTelaraña>();
        if (p != null) p.direccion = direccion;

        if (audioSource != null && disparoClip != null) audioSource.PlayOneShot(disparoClip);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        if (puntoDisparo != null) Gizmos.DrawLine(transform.position, puntoDisparo.position);
    }
}
