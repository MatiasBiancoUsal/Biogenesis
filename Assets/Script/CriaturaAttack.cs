using UnityEngine;

public class CriaturaAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilNormal;
    public GameObject proyectilMutado1;
    public GameObject proyectilMutadoFinal;
    public Transform firePoint;
    public float attackCooldown = 1f;
    public float rango = 6f;             // Rango configurable

    [Header("Animación")]
    public Animator animator;            // referencia al Animator

    [Header("Audio")]
    public AudioSource audioSource;      // Componente de audio
    public AudioClip disparoClip;        // Sonido del disparo

    [HideInInspector] public Transform currentTarget;

    public float cooldownTimer = 0f;
    private MutacionMutante mutacion;    // referencia al script de mutación

    void Start()
    {
        mutacion = GetComponent<MutacionMutante>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Detectar enemigo más cercano dentro del rango
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rango);
        currentTarget = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador"))
            {
                currentTarget = hit.transform;
                break;
            }
        }

        //CAMBIO SOO
        if (currentTarget != null)
        {
            FlipTowardsTarget(); // siempre mirar al objetivo

            if (cooldownTimer <= 0f)
            {
                Attack();
                cooldownTimer = attackCooldown;
            }
        }
        else if (animator != null)
        {
            animator.ResetTrigger("ataque1");
            animator.SetTrigger("Idle");
        }
    }

    //NUEVO SOO
    void FlipTowardsTarget()
    {
        if (currentTarget == null) return;

        // si el enemigo está a la izquierda, el mutante mira a la izquierda
        if (currentTarget.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("ataque1"); // activa la animación de ataque
        }

        // Disparo sincronizado con la animación
        Invoke(nameof(Shoot), 0.3f); // ajusta este delay al momento exacto del ataque
    }

    void Shoot()
    {
        if (firePoint == null || currentTarget == null) return;

        // Elegir proyectil según la mutación
        GameObject prefab = proyectilNormal;

        if (mutacion != null)
        {
            if (mutacion.estaEnMutacionFinal()) prefab = proyectilMutadoFinal;
            else if (mutacion.estaEnMutacion1()) prefab = proyectilMutado1;
        }

        if (prefab == null) return;

        GameObject proj = Instantiate(prefab, firePoint.position, Quaternion.identity);
        ProyectilCriatura p = proj.GetComponent<ProyectilCriatura>();

        if (p != null)
        {
            Vector2 dir = (currentTarget.position - firePoint.position).normalized;
            Debug.Log("Dirección proyectil: " + dir);  //ver en consola
            p.SetDirection(dir);
        }
        else
        {
            Debug.LogError("El proyectil no tiene ProyectilCriatura!!!");
        }


        // 🔊 Reproducir sonido de disparo
        PlayShootSound();
    }

    // --------------------
    // Audio
    // --------------------
    void PlayShootSound()
    {
        if (audioSource != null && disparoClip != null)
        {
            audioSource.PlayOneShot(disparoClip);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rango);
    }
}