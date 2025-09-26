using UnityEngine;

public class CriaturaAttack : MonoBehaviour
{
    [Header("Ataque")]
    public GameObject proyectilNormal;
    public GameObject proyectilMutado1;
    public GameObject proyectilMutadoFinal;
    public Transform firePoint;
    public float attackCooldown = 1f;
    public float rango = 6f;

    [Header("Animación")]
    public Animator animator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip disparoClip;

    [HideInInspector] public Transform currentTarget;

    public float cooldownTimer = 0f;
    private MutacionMutante mutacion;

    void Start()
    {
        mutacion = GetComponent<MutacionMutante>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // detectar enemigos
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
            FlipTowardsTarget();

            if (cooldownTimer <= 0f)
            {
                Attack();
                cooldownTimer = attackCooldown;
            }
        }
        else if (animator != null)
        {
            animator.ResetTrigger("ataque1");
            animator.ResetTrigger("ataque2");
            animator.ResetTrigger("ataque3");
        }
    }

    void FlipTowardsTarget()
    {
        if (currentTarget == null) return;

        if (currentTarget.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    void Attack()
    {
        string trigger = "ataque1"; // por defecto
        if (mutacion != null)
        {
            if (mutacion.estaEnMutacionFinal()) trigger = "ataque3";
            else if (mutacion.estaEnMutacion1()) trigger = "ataque2";
        }

        if (animator != null)
        {
            animator.ResetTrigger("ataque1");
            animator.ResetTrigger("ataque2");
            animator.ResetTrigger("ataque3");
            animator.SetTrigger(trigger);
        }

        Invoke(nameof(Shoot), 0.3f);
    }

    void Shoot()
    {
        if (firePoint == null || currentTarget == null) return;

        GameObject prefab = proyectilNormal;
        if (mutacion != null)
        {
            if (mutacion.estaEnMutacionFinal()) prefab = proyectilMutadoFinal;
            else if (mutacion.estaEnMutacion1()) prefab = proyectilMutado1;
        }

        if (prefab == null) return;

        GameObject proj = Instantiate(prefab, firePoint.position, Quaternion.identity);
        ProyectilMutante p = proj.GetComponent<ProyectilMutante>();

        if (p != null)
        {
            Vector2 dir = (currentTarget.position - firePoint.position).normalized;
            Debug.Log("Dirección proyectil: " + dir);
            p.SetDirection(dir);
        }
        else
        {
            Debug.LogError("El proyectil no tiene ProyectilMutante!!!");
        }

        PlayShootSound();
    }

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
