using UnityEngine;
using System.Collections;

public class ArañaDefensa : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;
    public float intervaloDisparo = 1.5f;
    public float rangoDeteccion = 6f;

    private bool disparando = false;
    private DerivadoAutoMover movimiento;

    float initialScaleX;
    float leftScaleX;
    float rightScaleX;
    float initialFirePointLocalX;

    [Header("Orientación")]
    [Tooltip("Marca true si el sprite en el editor APUNTA A LA IZQUIDA por defecto. Si al revés, marca false.")]
    public bool spriteFacesLeftByDefault = true;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip disparoClip;

    // --- NUEVO ---
    // Referencia al script del personaje para saber su estado.
    private Personaje personaje;

    void Start()
    {
        movimiento = GetComponent<DerivadoAutoMover>();

        // --- NUEVO ---
        // Obtenemos el componente Personaje al iniciar.
        personaje = GetComponent<Personaje>();

        initialScaleX = transform.localScale.x;
        leftScaleX = spriteFacesLeftByDefault ? Mathf.Abs(initialScaleX) : -Mathf.Abs(initialScaleX);
        rightScaleX = -leftScaleX;

        if (puntoDisparo != null)
            initialFirePointLocalX = puntoDisparo.localPosition.x;
    }

    void Update()
    {
        // --- NUEVO Y MUY IMPORTANTE ---
        // Si el personaje está incapacitado (vida <= 0), detenemos la lógica de ataque.
        if (personaje != null && personaje.vida <= 0)
        {
            // Opcional: nos aseguramos de que deje de intentar moverse o atacar.
            if (movimiento != null) movimiento.quieto = false;
            return; // No se ejecuta nada más del Update.
        }

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
                ForceFaceLeft();
                return hit.gameObject;
            }
        }

        if (movimiento != null) movimiento.quieto = false;
        return null;
    }

    IEnumerator DispararRoutine(Transform objetivo)
    {
        disparando = true;

        while (objetivo != null)
        {
            // --- NUEVO ---
            // Comprobamos la vida también dentro del bucle para detenerlo a mitad.
            if (personaje != null && personaje.vida <= 0)
            {
                break; // Sale del bucle de disparo si muere.
            }

            ForceFaceLeft();
            Disparar(objetivo);
            yield return new WaitForSeconds(intervaloDisparo);
        }

        disparando = false;
    }

    // ...el resto de tu código (ForceFaceLeft, Disparar, etc.) se mantiene igual...

    void ForceFaceLeft()
    {
        Vector3 s = transform.localScale;
        s.x = leftScaleX;
        transform.localScale = s;
        if (puntoDisparo != null)
            puntoDisparo.localPosition = new Vector3(initialFirePointLocalX, puntoDisparo.localPosition.y, puntoDisparo.localPosition.z);
    }

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