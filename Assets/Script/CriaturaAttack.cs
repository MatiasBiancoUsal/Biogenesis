using UnityEngine;

public class CriaturaAttack : MonoBehaviour
{
    [Header("Ataque")]
    //tina
    public GameObject proyectilNormal;
    public GameObject proyectilMutado1;
    public GameObject proyectilMutadoFinal;
    //
    //public GameObject proyectilPrefab;
    public Transform firePoint;
    public float attackCooldown = 1f;
    public float rango = 6f;             // Rango configurable

    [Header("Animación")]
    public Animator animator;            // referencia al Animator

    [HideInInspector] public Transform currentTarget;

    public float cooldownTimer = 0f;
    //tina
    private MutacionMutante mutacion; // referencia al script de mutación



    void Start()
    {
        mutacion = GetComponent<MutacionMutante>();
    }

    //

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

        //tina
        if (currentTarget != null && cooldownTimer <= 0f)
        {
            Attack();
            cooldownTimer = attackCooldown;
        }
        else if (currentTarget == null && animator != null)
        {
            animator.ResetTrigger("ataque1");
            animator.SetTrigger("Idle");
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("ataque1"); //  activa la animación de ataque
        }

        // Disparo sincronizado con la animación
        Invoke(nameof(Shoot), 0.3f); //  ajusta este delay al momento exacto del ataque
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
            p.SetDirection(dir);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rango);
    }
}
