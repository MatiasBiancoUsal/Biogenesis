using UnityEngine;

public class CriaturaAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;
    public Transform firePoint;
    public float attackCooldown = 1f;
    public float rango = 6f;             // Rango configurable

    [Header("Animaci�n")]
    public Animator animator;            // referencia al Animator

    [HideInInspector] public Transform currentTarget;

    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Detectar enemigo m�s cercano dentro del rango
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

        if (currentTarget != null)
        {
            // Atacar si el cooldown termin�
            if (cooldownTimer <= 0f)
            {
                Attack();
                cooldownTimer = attackCooldown;
            }
        }
        else
        {
            // Si no hay enemigos  Idle
            if (animator != null)
            {
                animator.ResetTrigger("ataque1");
                animator.SetTrigger("Idle");
            }
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("ataque1"); //  activa la animaci�n de ataque
        }

        // Disparo sincronizado con la animaci�n
        Invoke(nameof(Shoot), 0.3f); //  ajusta este delay al momento exacto del ataque
    }

    void Shoot()
    {
        if (proyectilPrefab == null || firePoint == null || currentTarget == null) return;

        GameObject proj = Instantiate(proyectilPrefab, firePoint.position, Quaternion.identity);
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
