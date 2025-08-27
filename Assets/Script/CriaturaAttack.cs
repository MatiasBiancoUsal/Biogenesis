using UnityEngine;

public class CriaturaAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilPrefab;   // Prefab del proyectil
    public Transform firePoint;          // Origen del disparo
    public float attackCooldown = 1f;    // Tiempo entre disparos
    public float rango = 6f;             // Rango de ataque configurable

    private float cooldownTimer = 0f;
    private Transform currentTarget;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Buscar enemigo más cercano dentro del rango
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, rango);
        currentTarget = null;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("depredador"))  // CORREGIDO a minúscula
            {
                currentTarget = hit.transform;
                break;
            }
        }

        // Si hay enemigo y pasó el cooldown, disparar
        if (currentTarget != null && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = attackCooldown;
        }
    }

    void Shoot()
    {
        if (proyectilPrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(proyectilPrefab, firePoint.position, Quaternion.identity);
        ProyectilCriatura p = proj.GetComponent<ProyectilCriatura>();
        if (p != null)
        {
            Vector2 dir = (currentTarget.position - firePoint.position).normalized;
            p.SetDirection(dir);
        }
    }

    // Visualizar rango en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rango);
    }
}
